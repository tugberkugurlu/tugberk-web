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
using Tugberk.Domain.Persistence;
using Tugberk.Domain.ReadSide;
using Tugberk.Domain.ReadSide.Queries;

namespace Tugberk.Persistance.SqlServer.Repositories
{
    public class PostsSqlServerRepository : 
        IPostsRepository, IApprovedPostsByIdQuery,
        IApprovedPostsBySlugQuery, IApprovedPostsByTagQuery,
        ILatestApprovedPostsQuery
    {
        private readonly BlogDbContext _blogDbContext;

        public PostsSqlServerRepository(BlogDbContext blogDbContext)
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
                .Where(x => x.ApprovalStatus == ApprovalStatusEntity.Approved);

            return await EvaluateQuery(skip, take, postsQuery);
        }

        public async Task<Paginated<Post>> GetLatestApprovedPosts(string tagSlug, int skip, int take)
        {
            // TODO: This query sucks, most of this query is evaluated locally and performance is at the bottom!
            var postsQuery = CreateBasePostQuery()
                .Where(x => x.ApprovalStatus == ApprovalStatusEntity.Approved)
                .Where(x => x.Tags.Any(t => t.Tag.Slug == tagSlug));

            return await EvaluateQuery(skip, take, postsQuery);
        }

        public async Task<Paginated<Post>> GetLatestApprovedPosts(int month, int year, int skip, int take)
        {
            var startDate = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endDate = new DateTime(month == 12 ? year + 1 : year, month == 12 ? 1 : month + 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var postsQuery = CreateBasePostQuery()
                .Where(x => x.ApprovalStatus == ApprovalStatusEntity.Approved)
                .Where(x => x.CreatedOnUtc >= startDate && x.CreatedOnUtc < endDate);
            
            return await EvaluateQuery(skip, take, postsQuery);
        }

        public async Task<Post> CreatePost(CreatePostCommand createPostCommand)
        {
            var authorId = createPostCommand.CreatedBy.Id;
            var createdBy = await _blogDbContext.Users.SingleOrDefaultAsync(x => x.Id == authorId);
            var claims = await GetCreatorClaims(authorId);

            var slug = new PostSlugEntity
            {
                Path = createPostCommand.Slug,
                IsDefault = true,
                CreatedBy = createdBy,
                CreatedOnUtc = DateTime.UtcNow
            };

            var postEntity = new PostEntity
            {
                Title = createPostCommand.Title,
                Abstract = createPostCommand.Abstract,
                Content = createPostCommand.Content,
                Language = "en-US",
                Format = PostFormatEntity.Html,
                CreatedBy = createdBy,
                CreatedOnUtc = DateTime.UtcNow,
                CreationIpAddress = createPostCommand.IPAddress,
                Slugs = new List<PostSlugEntity> { slug },
                Tags = new Collection<PostTagEntity>(createPostCommand.Tags.Select(t => new PostTagEntity
                {
                    Tag = new TagEntity { Name = t }
                }).ToList()),
                ApprovalStatus = createPostCommand.Approved 
                    ? ApprovalStatusEntity.Approved
                    : ApprovalStatusEntity.Disapproved
            };

            await _blogDbContext.AddAsync(postEntity);
            await _blogDbContext.SaveChangesAsync();

            return postEntity.ToDomainModel(claims);
        }

        private async Task<Paginated<Post>> EvaluateQuery(int skip, int take, IQueryable<PostEntity> postsQuery)
        {
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