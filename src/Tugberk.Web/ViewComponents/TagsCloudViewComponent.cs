using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Persistance.Abstractions;
using Tugberk.Web.Controllers;

namespace Tugberk.Web.ViewComponents
{
    public class TagsCloudViewComponent : ViewComponent
    {
        private readonly ITagsStore _tagsStore;

        public TagsCloudViewComponent(ITagsStore tagsStore)
        {
            _tagsStore = tagsStore ?? throw new ArgumentNullException(nameof(tagsStore));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _tagsStore.GetAll();

            return View(tags.Select(x => new TagViewModel
            {
                Name = x.Name,
                Slug = x.Slugs.First(s => s.IsDefault).Path,
                PostsCount = x.PostsCount
            }));
        }
    }
}