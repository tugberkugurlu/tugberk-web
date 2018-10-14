using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Persistance.Abstractions;

namespace Tugberk.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostsStore _postsStore;

        public HomeController(IPostsStore postsStore)
        {
            _postsStore = postsStore;
        }

        public async Task<IActionResult> Index(int page) 
        {
            if(page < 0) 
            {
                return NotFound();
            }

            int skip = 5 * page;
            int take = 5;

            var result = await _postsStore.GetLatestApprovedPosts(skip, take);

            return View(result.Items);
        }
    }
}
