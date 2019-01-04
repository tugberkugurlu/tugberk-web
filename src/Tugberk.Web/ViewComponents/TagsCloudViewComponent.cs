using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Domain.Persistence;
using Tugberk.Web.Controllers;

namespace Tugberk.Web.ViewComponents
{
    public class TagsCloudViewComponent : ViewComponent
    {
        private readonly ITagsRepository _tagsRepository;

        public TagsCloudViewComponent(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository ?? throw new ArgumentNullException(nameof(tagsRepository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _tagsRepository.GetAll();

            return View(tags.Select(x => new TagViewModel
            {
                Name = x.Name,
                Slug = x.Slugs.First(s => s.IsDefault).Path,
                PostsCount = x.PostsCount
            }));
        }
    }
}