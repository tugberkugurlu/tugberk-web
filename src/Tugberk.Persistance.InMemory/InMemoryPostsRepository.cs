using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OneOf;
using Optional;
using Tugberk.Domain;
using Tugberk.Domain.Commands;
using Tugberk.Domain.Persistence;
using Tugberk.Domain.ReadSide;

namespace Tugberk.Persistance.InMemory
{
    public class InMemoryPostsRepository : IPostsRepository
    {
        public Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPostById(string id) 
        {
            var post = GetSamplePost1();

            return Task.FromResult(
                Option.Some<OneOf<Post, NotApprovedResult<Post>>>(post)
            );
        }

        public async Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPostBySlug(string postSlug) 
        {
            var post = (await GetLatestApprovedPosts(0, Int32.MaxValue))
                .Items
                .FirstOrDefault(x => x.Slugs.Any(s => s.Path.Equals(postSlug, StringComparison.OrdinalIgnoreCase)));

            if(post != null) 
            {
                var result = post.IsApproved ? 
                    OneOf<Post, NotApprovedResult<Post>>.FromT0(post) :
                    OneOf<Post, NotApprovedResult<Post>>.FromT1(new NotApprovedResult<Post>(post));

                return Option.Some<OneOf<Post, NotApprovedResult<Post>>>(result);
            }
            else 
            {
                return Option.None<OneOf<Post, NotApprovedResult<Post>>>();
            }
        }

        public Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take) 
        {
            throw new NotImplementedException();
        }

        public Task<Paginated<Post>> GetLatestApprovedPosts(int skip, int take)
        {
            return Task.FromResult(new Paginated<Post>(new[]
            {
                GetSamplePost1(),
                GetSamplePost2()
            }, skip, 2));
        }

        public Task<Paginated<Post>> GetLatestApprovedPosts(string tagSlug, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<Paginated<Post>> GetLatestApprovedPosts(int month, int year, int skip, int take)
        {
            throw new NotImplementedException();
        }

        private static Post GetSamplePost1()
        {
            const string path = "sample-post-html-1.txt";
            string content = GetPostContent(path);

            var samplePost = new Post();
            samplePost.Id = "ydy982d";
            samplePost.Title = "Defining What Good Looks Like for a Software Engineer";
            samplePost.Abstract = "What does good look like for a software engineer? This is a question you might be asking frequently to yourself and I tried to share my thoughts on the topic with this blog post.";
            samplePost.Language = "en-US";
            samplePost.Content = content;
            var user = new User
            {
                Id = "djidiweh",
                Name = "Tugberk Ugurlu"
            };

            samplePost.Authors = (new[] { user });

            samplePost.CommentStatusActions = new[]
            {
                new CommentStatusActionRecord
                {
                    Status = CommentableStatus.Enabled,
                    RecordedBy = user,
                    RecordedOn = DateTime.UtcNow
                }
            };

            samplePost.ApprovalStatus = ApprovalStatus.Approved;
            
            samplePost.CreationRecord = new ChangeRecord
            {
                RecordedBy = user,
                RecordedOn = DateTime.UtcNow,
                IpAddress = "127.0.0.1"
            };

            samplePost.Slugs = new [] 
            {
                new Slug 
                {
                    Path = "defining-what-good-looks-like-for-a-software-engineer",
                    IsDefault = true,
                    CreatedOn = DateTime.UtcNow
                }
            };

            samplePost.Tags = new[]
            {
                new Tag
                {
                    Name = "Software Engineer",
                    Slugs = new [] 
                    {
                        new Slug 
                        {
                            Path = "software-engineer",
                            IsDefault = true,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                },

                new Tag
                {
                    Name = "Software Development",
                    Slugs = new [] 
                    {
                        new Slug 
                        {
                            Path = "software-development",
                            IsDefault = true,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                }
            };

            return samplePost;
        }

        private static Post GetSamplePost2()
        {
            const string path = "sample-post-html-2.txt";
            string content = GetPostContent(path);

            var samplePost = new Post();
            samplePost.Id = "y9h9huj9u";
            samplePost.Title = "ASP.NET Core Authentication in a Load Balanced Environment with HAProxy and Redis";
            samplePost.Abstract = "Token based authentication is a fairly common way of authenticating a user for an HTTP application. However, handling this in a load balanced environment has always involved extra caring. In this post, I will show you how this is handled in ASP.NET Core by demonstrating it with HAProxy and Redis through the help of Docker.";
            samplePost.Language = "en-US";
            samplePost.Content = content;
            var user = new User
            {
                Id = "djidiweh",
                Name = "Tugberk Ugurlu"
            };

            samplePost.Authors = (new[] { user });

            samplePost.CommentStatusActions = new[]
            {
                new CommentStatusActionRecord
                {
                    Status = CommentableStatus.Enabled,
                    RecordedBy = user,
                    RecordedOn = DateTime.UtcNow
                }
            };

            samplePost.ApprovalStatus = ApprovalStatus.Approved;

            samplePost.CreationRecord = new ChangeRecord
            {
                RecordedBy = user,
                RecordedOn = DateTime.UtcNow,
                IpAddress = "127.0.0.1"
            };

            samplePost.Slugs = new[]
            {
                new Slug
                {
                    Path = "asp-net-core-authentication-in-a-load-balanced-environment-with-haproxy-and-redis",
                    IsDefault = true,
                    CreatedOn = DateTime.UtcNow
                }
            };

            samplePost.Tags = new[]
            {
                new Tag
                {
                    Name = "ASP.NET Core",
                    Slugs = new []
                    {
                        new Slug
                        {
                            Path = "asp-net-core",
                            IsDefault = true,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                },

                new Tag
                {
                    Name = "ASP.NET",
                    Slugs = new []
                    {
                        new Slug
                        {
                            Path = "asp-net",
                            IsDefault = true,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                },

                new Tag
                {
                    Name = "Security",
                    Slugs = new []
                    {
                        new Slug
                        {
                            Path = "security",
                            IsDefault = true,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                },

                new Tag
                {
                    Name = "HTTP",
                    Slugs = new []
                    {
                        new Slug
                        {
                            Path = "http",
                            IsDefault = true,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                },

                new Tag
                {
                    Name = "Docker",
                    Slugs = new []
                    {
                        new Slug
                        {
                            Path = "docker",
                            IsDefault = true,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                }
            };

            return samplePost;
        }

        private static string GetPostContent(string path)
        {
            var location = Assembly.GetEntryAssembly().Location;
            var directory = Path.GetDirectoryName(location);
            var fullPath = Path.Combine(directory, path);

            return File.ReadAllText(fullPath, Encoding.UTF8);
        }

        public Task<Post> CreatePost(CreatePostCommand createPostCommand)
        {
            throw new NotImplementedException();
        }
    }
}
