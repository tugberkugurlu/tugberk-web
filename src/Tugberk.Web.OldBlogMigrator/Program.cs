using System;
using System.Data.SqlClient;
using System.Linq;
using Bloggy.Domain.Entities;
using Raven.Client;
using Raven.Client.Document;

namespace Tugberk.Web.OldBlogMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var connStr = args[0];
            var userId = args[1];

            using (IDocumentStore store = RetrieveDocumentStore())
            using (IDocumentSession ses = store.OpenSession())
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                ses.Advanced.MaxNumberOfRequestsPerSession = int.MaxValue;

                var allPosts = ses.Query<BlogPost>().Take(Int32.MaxValue).Where(x => x.IsApproved).ToList();
                var allTags = allPosts.SelectMany(x => x.Tags).Distinct(new TagEquality());
                
                foreach (var tag in allTags)
                {
                    Console.WriteLine($"Inserting Tag '{tag.Name}'");
                    InsertTag(conn, tag, userId);
                }

                foreach (var blogPost in allPosts)
                {
                    Console.WriteLine($"Inserting '{blogPost.Title}' post, written @ {blogPost.CreatedOn}");
                    InsertPost(conn, blogPost, userId);
                }
            }
        }

        private static void InsertPost(SqlConnection conn, BlogPost blogPost, string userId)
        {
            var blogPostNewId = Guid.NewGuid().ToString();
            using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[Posts]
           ([Id]
           ,[Abstract]
           ,[Content]
           ,[CreatedById]
           ,[CreatedOnUtc]
           ,[CreationIpAddress]
           ,[Format]
           ,[Language]
           ,[Title])
     VALUES (@Id, @Abstract, @Content, @CreatedById, @CreatedOnUtc, @CreationIpAddress, @Format, @Language, @Title)",
                conn))
            {
                // TODO: Old Identifier?
                // TODO: Last updated date?
                
                cmd.Parameters.AddWithValue("Id", blogPostNewId);
                cmd.Parameters.AddWithValue("Abstract", blogPost.BriefInfo);
                cmd.Parameters.AddWithValue("Content", blogPost.Content);
                cmd.Parameters.AddWithValue("CreatedById", userId);
                cmd.Parameters.AddWithValue("CreatedOnUtc", blogPost.CreatedOn.UtcDateTime);
                cmd.Parameters.AddWithValue("CreationIpAddress", blogPost.CreationIp);
                cmd.Parameters.AddWithValue("Format", 2);
                cmd.Parameters.AddWithValue("Language", "en-US");
                cmd.Parameters.AddWithValue("Title", blogPost.Title);

                cmd.ExecuteNonQuery();
            }

            foreach (var blogPostTag in blogPost.Tags)
            {
                using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[PostTagEntity]
           ([PostId]
           ,[TagName])
     VALUES (@PostId, @TagName)", conn))
                {
                    cmd.Parameters.AddWithValue("PostId", blogPostNewId);
                    cmd.Parameters.AddWithValue("TagName", blogPostTag.Name);

                    cmd.ExecuteNonQuery();
                }
            }

            foreach (var blogPostSlug in blogPost.Slugs)
            {
                using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[PostSlugEntity]
           ([Path]
           ,[CreatedById]
           ,[CreatedOnUtc]
           ,[IsDefault]
           ,[OwnedById])
     VALUES (@Path, @CreatedById, @CreatedOnUtc, @IsDefault, @OwnedById)", conn))
                {
                    cmd.Parameters.AddWithValue("Path", blogPostSlug.Path);
                    cmd.Parameters.AddWithValue("CreatedById", userId);
                    cmd.Parameters.AddWithValue("CreatedOnUtc", blogPostSlug.CreatedOn.UtcDateTime);
                    cmd.Parameters.AddWithValue("IsDefault", blogPostSlug.IsDefault);
                    cmd.Parameters.AddWithValue("OwnedById", blogPostNewId);

                    cmd.ExecuteNonQuery();
                }
            }

            if (blogPost.IsApproved)
            {
                using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[PostApprovalStatusActionEntity]
           ([Id]
           ,[PostId]
           ,[RecordedById]
           ,[RecordedOnUtc]
           ,[Status])
     VALUES (@Id, @PostId, @RecordedById, @RecordedOnUtc, @Status)", conn))
                {
                    cmd.Parameters.AddWithValue("Id", Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("PostId", blogPostNewId);
                    cmd.Parameters.AddWithValue("RecordedById", userId);
                    cmd.Parameters.AddWithValue("RecordedOnUtc", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("Status", 1);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void InsertTag(SqlConnection conn, Tag tag, string userId)
        {
            using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[Tags]
           ([Name]
           ,[CreatedById]
           ,[CreatedOnUtc]
           ,[CreationIpAddress]
           ,[Slug]) VALUES(@Name, @CreatedById, @CreatedOnUtc, @CreationIpAddress, @Slug)", conn))
            {
                cmd.Parameters.AddWithValue("Name", tag.Name);
                cmd.Parameters.AddWithValue("CreatedById", userId);
                cmd.Parameters.AddWithValue("CreatedOnUtc", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("CreationIpAddress", "127.0.0.1");
                cmd.Parameters.AddWithValue("Slug", tag.Slug);

                cmd.ExecuteNonQuery();
            }
        }

        private static IDocumentStore RetrieveDocumentStore()
        {
            IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080/",
                DefaultDatabase = "Bloggy"
            }.Initialize();

            return store;
        }
    }
}
