using System.Threading.Tasks;
using OneOf;
using Optional;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Domain.ReadSide.Queries
{
    public interface IApprovedPostsByIdQuery
    {
        Task<Option<OneOf<PostReadModel, NotApprovedResult<PostReadModel>>>> FindApprovedPostById(string id);
    }
}