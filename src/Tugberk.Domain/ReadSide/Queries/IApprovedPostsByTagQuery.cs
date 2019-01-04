using System.Collections.Generic;
using System.Threading.Tasks;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Domain.ReadSide.Queries
{
    public interface IApprovedPostsByTagQuery
    {
        Task<IReadOnlyCollection<PostReadModel>> GetApprovedPostsByTag(string tagSlug, int skip, int take);
    }
}