﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tugberk.Domain;
using Tugberk.Domain.Commands;
using Tugberk.Persistance.Abstractions;

namespace Tugberk.Persistance.InMemory
{
    public class InMemoryPostsStore : IPostsStore
    {
        public Task<Result> FindApprovedPostById(string id) 
        {
            var post = GetSamplePost1();
            return Task.FromResult<Result>(new FoundResult<Post>(post));
        }

        public async Task<Result> FindApprovedPostBySlug(string postSlug) 
        {
            var post = (await GetLatestApprovedPosts(0, Int32.MaxValue))
                .FirstOrDefault(x => x.Slugs.Any(s => s.Path.Equals(postSlug, StringComparison.OrdinalIgnoreCase)));

            Result result;
            if(post != null) 
            {
                if(post.IsApproved) 
                {
                    result = new FoundResult<Post>(post);
                }
                else 
                {
                    result = new NotApprovedResult<Post>(post);
                }
            }
            else 
            {
                result = new NotFoundResult();
            }

            return result;
        }

        public Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take) 
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Post>> GetLatestApprovedPosts(int skip, int take)
        {
            return Task.FromResult<IReadOnlyCollection<Post>>(new [] 
            { 
                GetSamplePost1(),
                GetSamplePost2()
            });
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
            samplePost.Format = PostFormat.Html;
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

            samplePost.ApprovaleStatusActions = new[]
            {
                new ApprovalStatusActionRecord
                {
                    Status = ApprovalStatus.Approved,
                    RecordedBy = user,
                    RecordedOn = DateTime.UtcNow
                }
            };

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
            samplePost.Format = PostFormat.Html;
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

            samplePost.ApprovaleStatusActions = new[]
            {
                new ApprovalStatusActionRecord
                {
                    Status = ApprovalStatus.Approved,
                    RecordedBy = user,
                    RecordedOn = DateTime.UtcNow
                }
            };

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

        public Task<Post> CreatePost(NewPostCommand newPostCommand)
        {
            throw new NotImplementedException();
        }
    }
}
