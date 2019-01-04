using System.Threading.Tasks;
using OneOf;
using Optional;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Domain.ReadSide.Queries
{
    public interface IApprovedPostsBySlugQuery
    {
        Task<Option<OneOf<PostReadModel, NotApprovedResult<PostReadModel>>>> FindApprovedPostBySlug(string postSlug);
    }
}