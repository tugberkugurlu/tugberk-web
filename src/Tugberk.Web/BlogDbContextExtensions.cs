using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tugberk.Domain;
using Tugberk.Domain.Commands;
using Tugberk.Persistance.InMemory;
using Tugberk.Persistance.SqlServer;
using Tugberk.Persistance.SqlServer.Stores;

namespace Tugberk.Web
{
    public static class BlogDbContextExtensions 
    {
        public static void EnsureSeedData(this BlogDbContext context, UserManager<IdentityUser> userManager)
        {
            var defaultUser = new IdentityUser 
            {
                UserName = "default@example.com",
                Email = "default@example.com"
            };

            if(!context.Users.AnyAsync().Result)
            {
                userManager.CreateAsync(defaultUser, "P@asW0rd").Wait();
                userManager.AddClaimsAsync(defaultUser, new [] 
                {
                    new Claim(ClaimTypes.Name, "Default"),
                    new Claim(ClaimTypes.Surname, "Author")
                }).Wait();
            }

            if(!context.Posts.AnyAsync().Result) 
            {
                var inMemoryPostStore = new InMemoryPostsStore();
                var store = new PostsSqlServerStore(context);
                var posts = inMemoryPostStore.GetLatestApprovedPosts(0, 100).Result;
                var tags = posts.SelectMany(x => x.Tags);

                context.AddRange(tags.Select(tag => new TagEntity 
                    {
                        Name = tag.Name,
                        Slug = tag.Slugs.First().Path,
                        CreatedBy = defaultUser,
                        CreationIpAddress = "127.0.0.1",
                        CreatedOnUtc = DateTime.UtcNow
                    }));

                context.SaveChanges();

                foreach (var post in posts)
                {
                    var newPostCommand = new NewPostCommand(post.Title,
                        post.Abstract,
                        post.Content,
                        post.Language,
                        post.Format,
                        post.CreationRecord.IpAddress,
                        new User { Id = defaultUser.Id, Name = defaultUser.UserName },
                        post.Tags.Select(x => x.Name).ToList().AsReadOnly());
                    
                    store.CreatePost(newPostCommand).Wait();
                }
            }
        }
    }
}
