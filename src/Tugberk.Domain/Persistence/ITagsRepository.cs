using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tugberk.Domain.Persistence
{
    public interface ITagsRepository
    {
        Task<IReadOnlyCollection<Tag>> GetAll();
    }
}