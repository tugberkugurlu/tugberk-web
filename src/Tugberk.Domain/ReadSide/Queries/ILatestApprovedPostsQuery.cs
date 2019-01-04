using System.Threading.Tasks;

namespace Tugberk.Domain.ReadSide.Queries
{
    public interface ILatestApprovedPostsQuery
    {
        Task<Paginated<Post>> GetLatestApprovedPosts(int skip, int take);
        Task<Paginated<Post>> GetLatestApprovedPosts(string tagSlug, int skip, int take);
        Task<Paginated<Post>> GetLatestApprovedPosts(int month, int year, int skip, int take);
    }
}