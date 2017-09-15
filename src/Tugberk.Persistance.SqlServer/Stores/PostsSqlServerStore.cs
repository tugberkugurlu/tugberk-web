using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tugberk.Domain;
using Tugberk.Persistance.Abstractions;

namespace Tugberk.Persistance.SqlServer.Posts
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

                // TODO: Add authors projection
                // TODO: Add slugs projection
                // TODO: Add CommentStatusActionRecord projection
                // TODO: Add ApprovalStatusActionRecord projection

                var post = new Post 
                {
                    Id = postEntity.Id.ToString(),
                    Language = postEntity.Language,
                    Title = postEntity.Title,
                    Abstract = postEntity.Abstract,
                    Content = postEntity.Content,
                    Format = postEntity.Format.ToDomainModel(),
                    CreationRecord = new ChangeRecord 
                    {
                        RecordedBy = postEntity.CreatedBy.ToDomainModel(),
                        RecordedOn = postEntity.CreatedOnUtc,
                        IpAddress = postEntity.CreationIpAddress
                    }
                };

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
    }
}