using System.Collections.Generic;
using System.Threading.Tasks;
using Tugberk.Domain.Queries.ReadModels;

namespace Tugberk.Domain.Queries
{
    public interface ITagsQuery
    {
        Task<IReadOnlyCollection<TagReadModel>> GetAll();
    }
}