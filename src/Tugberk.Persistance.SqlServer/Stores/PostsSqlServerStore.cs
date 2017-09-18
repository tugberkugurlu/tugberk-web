using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            _blogDbContext = blogDbContext ?? throw new System.ArgumentNullException(nameof(blogDbContext));
        }

        public async Task<PostFindResult> FindApprovedPostById(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var guid = Guid.Parse(id);
            var postEntity = await _blogDbContext.Posts
                .Include(x => x.Tags)
                .ThenInclude((PostTagEntity x) => x.Tag)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == guid);

            PostFindResult result;
            if(postEntity == null) 
            {
                result = PostFindResult.Fail(PostFindFailureReason.DoesNotExist);
            }
            else 
            {
                // TODO: Check for confirmation case
                var post = postEntity.ToDomainModel();
                result = PostFindResult.Success(post);
            }

            return result;
        }

        public Task<PostFindResult> FindApprovedPostBySlug(string postSlug)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyCollection<Post>> GetLatestApprovedPosts(int skip, int take)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Post> CreatePost(NewPostCommand newPostCommand)
        {
            var createdBy = new IdentityUser { Id = newPostCommand.CreatedBy.Id };
            var postEntity = new PostEntity
            {
                Title = newPostCommand.Title,
                Abstract = newPostCommand.Abstract,
                Content = newPostCommand.Title,            
                Format = newPostCommand.Format.ToEntityModel(),
                CreatedBy = createdBy,
                CreatedOnUtc = DateTime.UtcNow,
                CreationIpAddress = newPostCommand.IPAddress,
                Tags = new Collection<PostTagEntity>(newPostCommand.Tags.Select(t => new PostTagEntity
                {
                    Tag = new TagEntity { Name = t }
                }).ToList())
            };

            await _blogDbContext.AddAsync(postEntity);
            await _blogDbContext.SaveChangesAsync();

            return postEntity.ToDomainModel();
        }
    }
}