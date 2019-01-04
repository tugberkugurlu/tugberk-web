using System.Threading.Tasks;
using OneOf;
using Optional;
using Tugberk.Domain.Persistence;

namespace Tugberk.Domain.Queries
{
    public interface IApprovedPostsByIdQuery
    {
        Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPostById(string id);
    }
}