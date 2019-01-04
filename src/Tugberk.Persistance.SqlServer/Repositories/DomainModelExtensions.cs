using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Persistance.SqlServer.Repositories
{
    public static class DomainModelExtensions 
    {
        public static UserReadModel ToDomainModel(this IdentityUser user, IEnumerable<Claim> authorClaims) => 
            new UserReadModel
            {
                Id = user.Id,
                Name = GetName(authorClaims)
            };

        private static string GetName(IEnumerable<Claim> authorClaims) 
        {
            var name = authorClaims.First(x => x.Type.Equals(ClaimTypes.Name, StringComparison.InvariantCultureIgnoreCase)).Value;
            var surname = authorClaims.First(x => x.Type.Equals(ClaimTypes.Surname, StringComparison.InvariantCultureIgnoreCase)).Value;

            return $"{name} {surname}";
        }
    }
}