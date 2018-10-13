using System;
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
            using (IDocumentStore store = RetrieveDocumentStore())
            using (IDocumentSession ses = store.OpenSession())
            {
                ses.Advanced.MaxNumberOfRequestsPerSession = int.MaxValue;
                var posts = ses.Query<BlogPost>().Take(Int32.MaxValue).Where(x => x.IsApproved).ToList();
                foreach (var blogPost in posts)
                {
                    Console.WriteLine($"Found '{blogPost.Title}' post, written @ {blogPost.CreatedOn}");
                }
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
