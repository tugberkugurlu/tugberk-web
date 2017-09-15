using Microsoft.AspNetCore.Mvc;

namespace Tugberk.Web.Controllers
{
    [Route("portal")]
    public class PortalController : Controller
    {
        public IActionResult Index() => View();

        [HttpGet("posts")]
        public IActionResult CreatePost() => View();

        [HttpGet("posts/{postId}")]
        public IActionResult EditPost(string postId) => View();
    }
}
