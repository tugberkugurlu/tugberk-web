using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tugberk.Domain;
using Tugberk.Persistance.Abstractions;

namespace Tugberk.Persistance.InMemory
{
    public class InMemoryPostsStore : IPostsStore
    {
        public Task<PostFindResult> FindApprovedPostById(string id) 
        {
            var post = GetSamplePost();
            return Task.FromResult(PostFindResult.Success(post));
        }

        public Task<PostFindResult> FindApprovedPostBySlug(string postSlug) 
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Post>> GetApprovedPostsByTag(string tagSlug, int skip, int take) 
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Post>> GetLatestApprovedPosts(int skip, int take)
        {
            var post = GetSamplePost();
            return Task.FromResult<IReadOnlyCollection<Post>>(new [] { post });
        }

        private static Post GetSamplePost()
        {
            var location = Assembly.GetEntryAssembly().Location;
            var directory = Path.GetDirectoryName(location);
            var samplePostPath = Path.Combine(directory, "sampe-post-html.txt");
            var samplePost = new Post();
            samplePost.Id = "ydy982d";
            samplePost.Title = "Defining What Good Looks Like for a Software Engineer";
            samplePost.Abstract = "What does good look like for a software engineer? This is a question you might be asking frequently to yourself and I tried to share my thoughts on the topic with this blog post.";
            samplePost.Language = "en-US";
            samplePost.Format = PostFormat.Html;
            samplePost.Content = File.ReadAllText(samplePostPath, Encoding.UTF8);
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
    }
}
