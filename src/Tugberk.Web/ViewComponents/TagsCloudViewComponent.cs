using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Domain.ReadSide.Queries;
using Tugberk.Web.Controllers;

namespace Tugberk.Web.ViewComponents
{
    public class TagsCloudViewComponent : ViewComponent
    {
        private readonly ITagsQuery _tagsQuery;

        public TagsCloudViewComponent(ITagsQuery tagsQuery)
        {
            _tagsQuery = tagsQuery ?? throw new ArgumentNullException(nameof(tagsQuery));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _tagsQuery.GetAll();

            return View(tags.Select(x => new TagViewModel
            {
                Name = x.Name,
                Slug = x.Slugs.First(s => s.IsDefault).Path,
                PostsCount = x.PostsCount
            }));
        }
    }
}