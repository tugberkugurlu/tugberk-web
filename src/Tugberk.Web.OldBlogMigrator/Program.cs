using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Bloggy.Domain.Entities;
using Raven.Client;
using Raven.Client.Document;

namespace Tugberk.Web.OldBlogMigrator
{
    class Program
    {
        private class TagEquality : IEqualityComparer<Tag>
        {
            public bool Equals(Tag x, Tag y) => x.Name.Equals(y.Name);
            public int GetHashCode(Tag obj) => obj.Name.GetHashCode();
        }

        static void Main(string[] args)
        {
            var connStr = args[0];

            using (IDocumentStore store = RetrieveDocumentStore())
            using (IDocumentSession ses = store.OpenSession())
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                ses.Advanced.MaxNumberOfRequestsPerSession = int.MaxValue;
                var userId = Guid.NewGuid().ToString();

                var allPosts = ses.Query<BlogPost>().Take(Int32.MaxValue).Where(x => x.IsApproved).ToList();
                var allTags = allPosts.SelectMany(x => x.Tags).Distinct(new TagEquality());

                foreach (var tag in allTags)
                {
                    InsertTag(conn, tag, userId);
                }

                foreach (var blogPost in allPosts)
                {
                    Console.WriteLine($"Found '{blogPost.Title}' post, written @ {blogPost.CreatedOn}");
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
