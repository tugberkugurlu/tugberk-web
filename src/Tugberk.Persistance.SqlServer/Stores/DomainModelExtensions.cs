using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Tugberk.Domain;

namespace Tugberk.Persistance.SqlServer.Stores
{
    public static class DomainModelExtensions 
    {
        public static User ToDomainModel(this IdentityUser user, IEnumerable<IdentityUserClaim<string>> authorClaims) => 
            new User
            {
                Id = user.Id,
                Name = GetName(authorClaims)
            };

        private static string GetName(IEnumerable<IdentityUserClaim<string>> authorClaims) 
        {
            var name = authorClaims.First(x => x.ClaimType.Equals(ClaimTypes.Name, StringComparison.InvariantCultureIgnoreCase)).ClaimValue;
            var surname = authorClaims.First(x => x.ClaimType.Equals(ClaimTypes.Surname, StringComparison.InvariantCultureIgnoreCase)).ClaimValue;

            return $"{name} {surname}";
        }
    }
}