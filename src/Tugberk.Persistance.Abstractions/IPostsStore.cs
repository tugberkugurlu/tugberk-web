using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tugberk.Domain;
using Tugberk.Domain.Commands;

namespace Tugberk.Persistance.Abstractions
{
    public interface IPostsStore
    {
        Task<Result> FindApprovedPostById(string id);
        Task<Result> FindApprovedPostBySlug(string postSlug);
        Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take);
        Task<IReadOnlyCollection<Post>> GetLatestApprovedPosts(int skip, int take);

        Task<Post> CreatePost(NewPostCommand newPostCommand);
    }
}
