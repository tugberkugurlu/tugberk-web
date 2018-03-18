using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tugberk.Domain;
using Tugberk.Domain.Commands;

namespace Tugberk.Persistance.Abstractions
{
    public interface IPostsStore
    {
        Task<Either<Either<FoundResult<Post>, NotApprovedResult<Post>>, NotFoundResult>> FindApprovedPostById(string id);
        Task<Either<Either<FoundResult<Post>, NotApprovedResult<Post>>, NotFoundResult>> FindApprovedPostBySlug(string postSlug);
        Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take);
        Task<IReadOnlyCollection<Post>> GetLatestApprovedPosts(int skip, int take);

        Task<Post> CreatePost(NewPostCommand newPostCommand);
    }
}
