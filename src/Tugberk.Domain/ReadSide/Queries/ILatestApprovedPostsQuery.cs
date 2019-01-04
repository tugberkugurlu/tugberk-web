using System.Threading.Tasks;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Domain.ReadSide.Queries
{
    public interface ILatestApprovedPostsQuery
    {
        Task<Paginated<PostReadModel>> GetLatestApprovedPosts(int skip, int take);
        Task<Paginated<PostReadModel>> GetLatestApprovedPosts(string tagSlug, int skip, int take);
        Task<Paginated<PostReadModel>> GetLatestApprovedPosts(int month, int year, int skip, int take);
    }
}