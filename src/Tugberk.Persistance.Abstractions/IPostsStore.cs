using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneOf;
using Optional;
using Tugberk.Domain;
using Tugberk.Domain.Commands;

namespace Tugberk.Persistance.Abstractions
{
    public interface IPostsStore
    {
        Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPostById(string id);
        Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPostBySlug(string postSlug);
        Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take);
        Task<IReadOnlyCollection<Post>> GetLatestApprovedPosts(int skip, int take);

        Task<Post> CreatePost(NewPostCommand newPostCommand);
    }
}
