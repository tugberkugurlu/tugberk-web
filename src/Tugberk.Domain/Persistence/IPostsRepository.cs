using System.Threading.Tasks;
using Tugberk.Domain.Commands;

namespace Tugberk.Domain.Persistence
{
    public interface IPostsRepository
    {
        Task<Post> CreatePost(NewPostCommand newPostCommand);
    }
}
