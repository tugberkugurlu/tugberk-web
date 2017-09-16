using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Web.Models;

namespace Tugberk.Web.Controllers
{
    [Route("portal")]
    public class PortalController : Controller
    {
        private readonly IImageStorage _imageStorage;

        public PortalController(IImageStorage imageStorage)
        {
            _imageStorage = imageStorage ?? throw new System.ArgumentNullException(nameof(imageStorage));
        }

        public IActionResult Index() => View();

        [HttpGet("posts/create")]
        public IActionResult CreatePost() => View();

        [ValidateAntiForgeryToken]
        [HttpPost("posts/create")]
        public IActionResult CreatePost(NewPostRequestModel requestModel)
        {
            if(ModelState.IsValid) 
            {
                // TODO: try to save blog post here
            }

            return View(requestModel);
        }

        [HttpGet("posts/{postId}")]
        public IActionResult EditPost(string postId) => View();

        [ValidateAntiForgeryToken]
        [HttpPost("images/upload")]
        public async Task<IActionResult> UploadImage()
        {
            if(Request.Form.Files.Count < 1)
            {
                return BadRequest("Request contains no file to upload");
            }

            if(Request.Form.Files.Count > 1)
            {
                return BadRequest("More than one file to upload at the same time is not supported");
            }

            var firstFile = Request.Form.Files.First();

            using(var stream = firstFile.OpenReadStream())
            {
                var result = await _imageStorage.SaveImage(stream, firstFile.FileName);

                return Json(new 
                { 
                    ImageUrl = result.Url
                });
            }
        }
    }
}
