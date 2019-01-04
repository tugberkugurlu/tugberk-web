using System.Collections.Generic;
using System.Threading.Tasks;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Domain.ReadSide.Queries
{
    public interface ITagsQuery
    {
        Task<IReadOnlyCollection<TagReadModel>> GetAll();
    }
}