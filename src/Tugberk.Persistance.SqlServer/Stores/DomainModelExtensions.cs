using Microsoft.AspNetCore.Identity;
using Tugberk.Domain;

namespace Tugberk.Persistance.SqlServer.Stores
{
    public static class DomainModelExtensions 
    {
        public static User ToDomainModel(this IdentityUser user) => 
            new User
            {
                Id = user.Id,
                Name = user.UserName
            };
    }
}