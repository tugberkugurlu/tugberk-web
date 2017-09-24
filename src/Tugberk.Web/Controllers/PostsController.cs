using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Persistance.Abstractions;

namespace Tugberk.Web.Controllers
{
    public class PostsController : Controller 
    {
        private readonly IPostsStore _postsStore;
        private readonly PostResultHttpHandleStragety _postResultHttpHandleStragety;

        public PostsController(IPostsStore postsStore)
        {
            _postResultHttpHandleStragety = new PostResultHttpHandleStragety(this);
            _postsStore = postsStore;
        }

        [HttpGet("archive/{slug}")]
        public async Task<IActionResult> Index(string slug)
        {
            var result = await _postsStore.FindApprovedPostBySlug(slug);
            return _postResultHttpHandleStragety.HandleResult(result);
        }
    }
}
