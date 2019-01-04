using System.Collections.Generic;
using System.Threading.Tasks;
using OneOf;
using Optional;
using Tugberk.Domain.Commands;

namespace Tugberk.Domain.Persistence
{
    public interface IPostsRepository
    {
        Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPostById(string id);
        Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPostBySlug(string postSlug);
        Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take);
        Task<Paginated<Post>> GetLatestApprovedPosts(int skip, int take);
        Task<Paginated<Post>> GetLatestApprovedPosts(string tagSlug, int skip, int take);
        Task<Paginated<Post>> GetLatestApprovedPosts(int month, int year, int skip, int take);

        Task<Post> CreatePost(NewPostCommand newPostCommand);
    }
}
