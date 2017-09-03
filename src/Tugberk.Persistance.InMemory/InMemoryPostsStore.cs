using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tugberk.Domain;
using Tugberk.Persistance.Abstractions;

namespace Tugberk.Persistance.InMemory
{
    public class InMemoryPostsStore : IPostsStore
    {
        public Task<PostFindResult> FindApprovedPostById(string id) 
        {
            throw new NotImplementedException();
        }

        public Task<PostFindResult> FindApprovedPostBySlug(string postSlug) 
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take) 
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Post>> GetLatestApprovedPosts(int skip, int take) 
        {
            throw new NotImplementedException();
        }
    }
}
