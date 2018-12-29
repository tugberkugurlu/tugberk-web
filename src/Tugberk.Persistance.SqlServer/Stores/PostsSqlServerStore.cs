using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Optional;
using Tugberk.Domain;
using Tugberk.Domain.Commands;
using Tugberk.Persistance.Abstractions;

namespace Tugberk.Persistance.SqlServer.Stores
{
    public class PostsSqlServerStore : IPostsStore
    {
        private readonly BlogDbContext _blogDbContext;

        public PostsSqlServerStore(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext ?? throw new ArgumentNullException(nameof(blogDbContext));
        }

        public Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPostById(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var guid = Guid.Parse(id);

            return FindApprovedPost(x => x.Id == guid);
        }

        public Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPostBySlug(string postSlug)
        {
            if (postSlug == null) throw new ArgumentNullException(nameof(postSlug));

            return FindApprovedPost(x => x.Slugs.Any(s => s.Path == postSlug));
        }

        public Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Paginated<Post>> GetLatestApprovedPosts(int skip, int take)
        {
            // TODO: This query sucks, most of this query is evaluated locally and performance is at the bottom!
            var postsQuery = CreateBasePostQuery()
                // TODO: The performance issue is related to the way we evaluate approval. Skip this for now till we fixed the modal.
                //.Where(x => 
                //    x.ApprovalStatusActions.Any(a => a.Status == ApprovalStatusEntity.Approved) && 
                //    x.ApprovalStatusActions.OrderByDescending(a => a.RecordedOnUtc).First().Status == ApprovalStatusEntity.Approved)
                ;

            var posts = await postsQuery
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalCount = await postsQuery.CountAsync();

            // TODO: This is obviously not great but EF sucks and was not able to generate the correct query to get this out from SQL Server. Fix this or cache.
            var userIds = posts.Select(x => x.CreatedBy.Id).Distinct(StringComparer.InvariantCultureIgnoreCase);
            var claims = await _blogDbContext.UserClaims
                .Where(x => userIds.Any(id => x.UserId == id))
                .ToListAsync();

            var result = new ReadOnlyCollection<Post>(posts.Select(x =>
                x.ToDomainModel(claims
                    .Where(c => c.UserId.Equals(x.CreatedBy.Id, StringComparison.InvariantCultureIgnoreCase))
                    .Select(cl => new Claim(cl.ClaimType, cl.ClaimValue)))).ToList());

            return new Paginated<Post>(result, skip, totalCount);
        }

        public async Task<Paginated<Post>> GetLatestApprovedPosts(string tagSlug, int skip, int take)
        {
            // TODO: This query sucks, most of this query is evaluated locally and performance is at the bottom!
            var postsQuery = CreateBasePostQuery()
                .Where(x => x.ApprovalStatus == ApprovalStatusEntity.Approved)
                .Where(x => x.Tags.Any(t => t.Tag.Slug == tagSlug));

            var posts = await postsQuery
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalCount = await postsQuery.CountAsync();

            // TODO: This is obviously not great but EF sucks and was not able to generate the correct query to get this out from SQL Server. Fix this or cache.
            var userIds = posts.Select(x => x.CreatedBy.Id).Distinct(StringComparer.InvariantCultureIgnoreCase);
            var claims = await _blogDbContext.UserClaims
                .Where(x => userIds.Any(id => x.UserId == id))
                .ToListAsync();

            var result = new ReadOnlyCollection<Post>(posts.Select(x =>
                x.ToDomainModel(claims
                    .Where(c => c.UserId.Equals(x.CreatedBy.Id, StringComparison.InvariantCultureIgnoreCase))
                    .Select(cl => new Claim(cl.ClaimType, cl.ClaimValue)))).ToList());

            return new Paginated<Post>(result, skip, totalCount);
        }

        public async Task<Post> CreatePost(NewPostCommand newPostCommand)
        {
            var authorId = newPostCommand.CreatedBy.Id;
            var createdBy = await _blogDbContext.Users.SingleOrDefaultAsync(x => x.Id == authorId);
            var claims = await GetCreatorClaims(authorId);

            var slug = new PostSlugEntity
            {
                Path = newPostCommand.Slug,
                IsDefault = true,
                CreatedBy = createdBy,
                CreatedOnUtc = DateTime.UtcNow
            };

            var postEntity = new PostEntity
            {
                Title = newPostCommand.Title,
                Abstract = newPostCommand.Abstract,
                Content = newPostCommand.Content,
                Language = newPostCommand.Language,
                Format = newPostCommand.Format.ToEntityModel(),
                CreatedBy = createdBy,
                CreatedOnUtc = DateTime.UtcNow,
                CreationIpAddress = newPostCommand.IPAddress,
                Slugs = new List<PostSlugEntity> { slug },
                Tags = new Collection<PostTagEntity>(newPostCommand.Tags.Select(t => new PostTagEntity
                {
                    Tag = new TagEntity { Name = t }
                }).ToList()),
                ApprovalStatus = newPostCommand.Approved 
                    ? ApprovalStatusEntity.Approved
                    : ApprovalStatusEntity.Disapproved
            };

            await _blogDbContext.AddAsync(postEntity);
            await _blogDbContext.SaveChangesAsync();

            return postEntity.ToDomainModel(claims);
        }

        private async Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPost(Expression<Func<PostEntity, bool>> predicate)
        {        
            var postEntity = await CreateBasePostQuery().FirstOrDefaultAsync(predicate);

            if (postEntity == null)
            {
                return Option.None<OneOf<Post, NotApprovedResult<Post>>>();
            }
            else
            {
                var claims = await GetCreatorClaims(postEntity.CreatedBy.Id);
                var post = postEntity.ToDomainModel(claims);
                var result = post.IsApproved ? 
                    OneOf<Post, NotApprovedResult<Post>>.FromT0(post) :
                    OneOf<Post, NotApprovedResult<Post>>.FromT1(new NotApprovedResult<Post>(post));

                return Option.Some(result);
            }
        }

        private async Task<IReadOnlyCollection<Claim>> GetCreatorClaims(string userId)
        {
            return (await _blogDbContext.UserClaims.Where(x => x.UserId == userId).ToListAsync())
                .Select(x => new Claim(x.ClaimType, x.ClaimValue))
                .ToList();
        }

        private IQueryable<PostEntity> CreateBasePostQuery() => _blogDbContext.Posts
            .Include(x => x.Slugs)
            .Include(x => x.Tags)
            .ThenInclude(x => x.Tag)
            .Include(x => x.CreatedBy)
            .OrderByDescending(x => x.CreatedOnUtc);
    }
}