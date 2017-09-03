using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tugberk.Domain;

namespace Tugberk.Persistance.Abstractions
{
    public interface IPostsStore
    {
        Task<Post> FindApprovedPostById(string id);
        Task<Post> FindApprovedPostBySlug(string postSlug);
        Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take);
        Task<IReadOnlyCollection<Post>> GetLatestApprovedPosts(int skip, int take);
    }
}
