using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        [ValidateAntiForgeryToken]
        [HttpPost("images/upload")]
        public async Task<IActionResult> UploadImage(IEnumerable<IFormFile> files)
        {
            await Task.Delay(2000);
            return Json(new 
            { 
                ImageUrl = "https://tugberkugurlu.blob.core.windows.net/bloggyimages/640px-Coding_Shots_Annual_Plan_high_res-5.jpg" 
            });
        }
    }
}
