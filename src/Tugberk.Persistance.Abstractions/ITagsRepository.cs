using System.Collections.Generic;
using System.Threading.Tasks;
using Tugberk.Domain;

namespace Tugberk.Persistance.Abstractions
{
    public interface ITagsRepository
    {
        Task<IReadOnlyCollection<Tag>> GetAll();
    }
}