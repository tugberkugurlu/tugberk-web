using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tugberk.Persistance.SqlServer;

namespace Tugberk.Web
{
    public static class BlogDbContextExtensions 
    {
        public static void EnsureSeedData(this BlogDbContext context, UserManager<IdentityUser> userManager)
        {
            if(!context.Users.AnyAsync().Result)
            {
                var defaultUser = new IdentityUser 
                {
                    UserName = "default@example.com",
                    Email = "default@example.com"
                };

                userManager.CreateAsync(defaultUser, "P@asW0rd").Wait();
            }
        }
    }
}
