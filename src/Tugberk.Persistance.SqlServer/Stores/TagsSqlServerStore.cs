using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tugberk.Domain;
using Tugberk.Persistance.Abstractions;

namespace Tugberk.Persistance.SqlServer.Stores
{
    public class TagsSqlServerStore : ITagsStore
    {
        private readonly BlogDbContext _blogDbContext;

        public TagsSqlServerStore(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext ?? throw new ArgumentNullException(nameof(blogDbContext));
        }

        public async Task<IReadOnlyCollection<Tag>> GetAll()
        {
            var tags = await _blogDbContext.Tags
                .Select(tag => new
                {
                    Tag = tag,
                    CountOfPosts = 0 // below is evaluated locally, EF sucks as hell!
                    // CountOfPosts = tag.Posts.Count()
                }).ToListAsync();

            return tags.Select(x => new Tag
            {
                Name = x.Tag.Name,
                Slugs = new[] 
                {
                    new Slug
                    {
                        Path = x.Tag.Slug,
                        CreatedOn = DateTime.UtcNow,
                        IsDefault = true
                    }
                },
                PostsCount = x.CountOfPosts
            }).ToList().AsReadOnly();
        }
    }
}