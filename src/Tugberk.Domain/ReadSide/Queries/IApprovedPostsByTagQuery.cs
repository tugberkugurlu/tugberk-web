using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tugberk.Domain.ReadSide.Queries
{
    public interface IApprovedPostsByTagQuery
    {
        Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take);
    }
}