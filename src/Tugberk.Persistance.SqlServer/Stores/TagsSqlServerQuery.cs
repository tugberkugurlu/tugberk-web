using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tugberk.Domain.ReadSide;
using Tugberk.Domain.ReadSide.Queries;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Persistance.SqlServer.Stores
{
    public class TagsSqlServerQuery : ITagsQuery
    {
        private readonly BlogDbContext _blogDbContext;

        public TagsSqlServerQuery(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext ?? throw new ArgumentNullException(nameof(blogDbContext));
        }

        public async Task<IReadOnlyCollection<TagReadModel>> GetAll()
        {
            var tags = await _blogDbContext.Tags
                .Select(tag => new
                {
                    Tag = tag,
                    CountOfPosts = 0 // below is evaluated locally, EF sucks as hell!
                    // CountOfPosts = tag.Posts.Count()
                }).ToListAsync();

            return tags.Select(x => new TagReadModel
            {
                Name = x.Tag.Name,
                Slugs = new[] 
                {
                    new SlugReadModel
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