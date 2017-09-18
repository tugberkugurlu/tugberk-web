using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tugberk.Persistance.SqlServer.Stores;
using Xunit;

namespace Tugberk.Persistance.SqlServer.Tests
{    
    public class PostsSqlServerStoreTests
    {
        [Fact]
        public async Task FindApprovedPostById_ShouldFindThePostWhenItExistsAndApproved()
        {
            using (var provider = BlogDbContextProvider.Create())
            {
                var postId = Guid.NewGuid();
                var expectedTitle = TestUtils.RandomString();
                var expectedAbstract = TestUtils.RandomString(40);

                using (var context = provider.CreateContext())
                {
                    const string ipAddress = "127.0.0.1";
                    string userId = Guid.NewGuid().ToString();
                    var user = new IdentityUser
                    {
                        Id = userId,
                        UserName = TestUtils.RandomString(),
                        Email = $"{TestUtils.RandomString()}@{TestUtils.RandomString()}.com"
                    };

                    var post = new PostEntity
                    {
                        Id = postId,
                        Language = "en-US",
                        Title = expectedTitle,
                        Abstract = expectedAbstract,
                        Content = TestUtils.RandomString(200),
                        Format = PostFormatEntity.PlainText,
                        CreatedBy = user,
                        CreatedOnUtc = DateTime.UtcNow,
                        CreationIpAddress = ipAddress,
                        Tags = new Collection<PostTagEntity>((new[]
                        {
                            TestUtils.RandomString(),
                            TestUtils.RandomString()
                        }).Select(t => new PostTagEntity
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

                    await context.Users.AddAsync(user);
                    await context.Posts.AddAsync(post);
                    await context.SaveChangesAsync();
                }

                using (var context = provider.CreateContext())
                {
                    var store = new PostsSqlServerStore(context);
                    var postFindResult = await store.FindApprovedPostById(postId.ToString());

                    Assert.True(postFindResult.IsSuccess);
                    Assert.Equal(postId.ToString(), postFindResult.Post.Id);
                    Assert.Equal(expectedTitle, postFindResult.Post.Title);
                    Assert.Equal(expectedAbstract, postFindResult.Post.Abstract);
                }
            }
        }
    }
}