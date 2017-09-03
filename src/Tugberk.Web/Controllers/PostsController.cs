using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Persistance.Abstractions;

namespace Tugberk.Web.Controllers
{
    public class PostsController : Controller 
    {
        private readonly IPostsStore _postsStore;

        public PostsController(IPostsStore postsStore)
        {
            _postsStore = postsStore;
        }

        [HttpGet("archive/{slug}")]
        public async Task<IActionResult> Index(string slug)
        {
            var result = await _postsStore.FindApprovedPostBySlug(slug);
            if(result.IsSuccess) 
            {
                // TODO: Convert to ViewModel here
                return View(result.Post);
            }

            return NotFound();
        }
    }
}
