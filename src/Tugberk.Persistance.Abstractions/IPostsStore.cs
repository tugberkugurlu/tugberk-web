using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tugberk.Domain;
using Tugberk.Domain.Commands;

namespace Tugberk.Persistance.Abstractions
{
    public interface IPostsStore
    {
        Task<PostFindResult> FindApprovedPostById(string id);
        Task<PostFindResult> FindApprovedPostBySlug(string postSlug);
        Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take);
        Task<IReadOnlyCollection<Post>> GetLatestApprovedPosts(int skip, int take);

        Task<Post> CreatePost(NewPostCommand newPostCommand);
    }
}
