using System.Threading.Tasks;
using Tugberk.Domain.Commands;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Domain.Persistence
{
    public interface IPostsRepository
    {
        Task<PostReadModel> CreatePost(CreatePostCommand createPostCommand);
    }
}
