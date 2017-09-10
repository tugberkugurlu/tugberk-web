using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tugberk.Persistance.SqlServer.Tests
{    
    public class PostStoreTests
    {
        // https://www.davepaquette.com/archive/2016/11/27/integration-testing-with-entity-framework-core-and-sql-server.aspx

        [Fact]
        public async Task ShouldSaveTheTagsOfThePostAsExpected()
        {
            var databaseName = $"TugberkWeb_{Path.GetRandomFileName()}";
            var connectionString = $"Server=localhost;Database={databaseName};User ID=sa;Password=Passw0rd;MultipleActiveResultSets=true";

            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<BlogDbContext>();
            builder.UseSqlServer(connectionString)
                .UseInternalServiceProvider(serviceProvider);

            using (var tempCtx = new BlogDbContext(builder.Options))
            {
                await tempCtx.Database.MigrateAsync();
            }

            string userId = Guid.NewGuid().ToString();
            using (var tempCtx = new BlogDbContext(builder.Options))
            {
                var user = new IdentityUser
                {
                    Id = userId,
                    UserName = TestUtils.RandomString(),
                    Email = $"{TestUtils.RandomString()}@{TestUtils.RandomString()}.com"
                };

                await tempCtx.Users.AddAsync(user);
                await tempCtx.SaveChangesAsync();
            }

            var postId = Guid.NewGuid();
            var tags = new [] 
            {
                TestUtils.RandomString(),
                TestUtils.RandomString()
            };

            try
            {
                using (var context = new BlogDbContext(builder.Options))
                {
                    const string ipAddress = "127.0.0.1";
                    var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                    var post = new PostEntity
                    {
                        Id = postId,
                        Language = "en-US",
                        Title = TestUtils.RandomString(),
                        Abstract = TestUtils.RandomString(40),
                        Content = TestUtils.RandomString(200),
                        Format = PostFormatEntity.PlainText,
                        CreatedBy = user,
                        CreatedOnUtc = DateTime.UtcNow,
                        CreationIpAddress = ipAddress,
                        Tags = new Collection<PostTagEntity>(tags.Select(t => new PostTagEntity 
                        {
                            Tag = new TagEntity
                            {
                                Name = t,
                                Slug = TestUtils.RandomString(),
                                CreatedBy = user,
                                CreatedOnUtc = DateTime.UtcNow,
                                CreationIpAddress = ipAddress
                            }
                        }).ToList())
                    };

                    await context.Posts.AddAsync(post);
                    await context.SaveChangesAsync();
                }

                using (var context = new BlogDbContext(builder.Options))
                {
                    // see https://docs.microsoft.com/en-us/ef/core/querying/related-data
                    var post = await context.Posts.Include(x => x.Tags)
                        .ThenInclude((PostTagEntity x) => x.Tag)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == postId);

                    var tagNames = post.Tags.Select(x => x.Tag.Name);

                    Assert.Equal(tags, tagNames);
                }
            }
            finally
            {
                using (var tempCtx = new BlogDbContext(builder.Options))
                {
                    await tempCtx.Database.EnsureDeletedAsync();
                }
            }
        }
    }
}
