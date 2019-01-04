using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Persistance.InMemory
{
    public class InMemoryPosts
    {
        public Task<Paginated<PostReadModel>> GetLatestApprovedPosts(int skip, int take)
        {
            return Task.FromResult(new Paginated<PostReadModel>(new[]
            {
                GetSamplePost1(),
                GetSamplePost2()
            }, skip, 2));
        }

        private static PostReadModel GetSamplePost1()
        {
            const string path = "sample-post-html-1.txt";
            string content = GetPostContent(path);

            var user = new UserReadModel
            {
                Id = "djidiweh",
                Name = "Tugberk Ugurlu"
            };

            var samplePost = new PostReadModel
            {
                Id = "ydy982d",
                Title = "Defining What Good Looks Like for a Software Engineer",
                Abstract = "What does good look like for a software engineer? This is a question you might be asking frequently to yourself and I tried to share my thoughts on the topic with this blog post.",
                Language = "en-US",
                Content = content,
                Authors = (new[] {user}),
                CommentStatusActions = new[]
                {
                    new CommentStatusActionRecordReadModel
                    {
                        Status = CommentableStatusReadModel.Enabled,
                        RecordedBy = user,
                        RecordedOn = DateTime.UtcNow
                    }
                },
                ApprovalStatus = ApprovalStatusReadModel.Approved,
                CreationRecord = new ChangeRecordReadModel
                {
                    RecordedBy = user,
                    RecordedOn = DateTime.UtcNow,
                    IpAddress = "127.0.0.1"
                },
                Slugs = new[]
                {
                    new SlugReadModel
                    {
                        Path = "defining-what-good-looks-like-for-a-software-engineer",
                        IsDefault = true,
                        CreatedOn = DateTime.UtcNow
                    }
                },
                Tags = new[]
                {
                    new TagReadModel
                    {
                        Name = "Software Engineer",
                        Slugs = new[]
                        {
                            new SlugReadModel
                            {
                                Path = "software-engineer",
                                IsDefault = true,
                                CreatedOn = DateTime.UtcNow
                            }
                        }
                    },

                    new TagReadModel
                    {
                        Name = "Software Development",
                        Slugs = new[]
                        {
                            new SlugReadModel
                            {
                                Path = "software-development",
                                IsDefault = true,
                                CreatedOn = DateTime.UtcNow
                            }
                        }
                    }
                }
            };

            return samplePost;
        }

        private static PostReadModel GetSamplePost2()
        {
            const string path = "sample-post-html-2.txt";
            string content = GetPostContent(path);

            var user = new UserReadModel
            {
                Id = "djidiweh",
                Name = "Tugberk Ugurlu"
            };

            var samplePost = new PostReadModel
            {
                Id = "y9h9huj9u",
                Title = "ASP.NET Core Authentication in a Load Balanced Environment with HAProxy and Redis",
                Abstract = "Token based authentication is a fairly common way of authenticating a user for an HTTP application. However, handling this in a load balanced environment has always involved extra caring. In this post, I will show you how this is handled in ASP.NET Core by demonstrating it with HAProxy and Redis through the help of Docker.",
                Language = "en-US",
                Content = content,
                Authors = new[] {user},
                CommentStatusActions = new[]
                {
                    new CommentStatusActionRecordReadModel
                    {
                        Status = CommentableStatusReadModel.Enabled,
                        RecordedBy = user,
                        RecordedOn = DateTime.UtcNow
                    }
                },
                ApprovalStatus = ApprovalStatusReadModel.Approved,
                CreationRecord = new ChangeRecordReadModel
                {
                    RecordedBy = user,
                    RecordedOn = DateTime.UtcNow,
                    IpAddress = "127.0.0.1"
                },
                Slugs = new[]
                {
                    new SlugReadModel
                    {
                        Path = "asp-net-core-authentication-in-a-load-balanced-environment-with-haproxy-and-redis",
                        IsDefault = true,
                        CreatedOn = DateTime.UtcNow
                    }
                },
                Tags = new[]
                {
                    new TagReadModel
                    {
                        Name = "ASP.NET Core",
                        Slugs = new[]
                        {
                            new SlugReadModel
                            {
                                Path = "asp-net-core",
                                IsDefault = true,
                                CreatedOn = DateTime.UtcNow
                            }
                        }
                    },

                    new TagReadModel
                    {
                        Name = "ASP.NET",
                        Slugs = new[]
                        {
                            new SlugReadModel
                            {
                                Path = "asp-net",
                                IsDefault = true,
                                CreatedOn = DateTime.UtcNow
                            }
                        }
                    },

                    new TagReadModel
                    {
                        Name = "Security",
                        Slugs = new[]
                        {
                            new SlugReadModel
                            {
                                Path = "security",
                                IsDefault = true,
                                CreatedOn = DateTime.UtcNow
                            }
                        }
                    },

                    new TagReadModel
                    {
                        Name = "HTTP",
                        Slugs = new[]
                        {
                            new SlugReadModel
                            {
                                Path = "http",
                                IsDefault = true,
                                CreatedOn = DateTime.UtcNow
                            }
                        }
                    },

                    new TagReadModel
                    {
                        Name = "Docker",
                        Slugs = new[]
                        {
                            new SlugReadModel
                            {
                                Path = "docker",
                                IsDefault = true,
                                CreatedOn = DateTime.UtcNow
                            }
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
    }
}
