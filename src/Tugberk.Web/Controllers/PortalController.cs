using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Domain.Commands;
using Tugberk.Domain.Persistence;
using Tugberk.Web.MediaStorage;
using Tugberk.Web.Models;

namespace Tugberk.Web.Controllers
{
    [Authorize]
    [Route("portal")]
    public class PortalController : Controller
    {
        private readonly IImageStorage _imageStorage;
        private readonly IPostsRepository _postsRepository;

        public PortalController(IImageStorage imageStorage, IPostsRepository postsRepository)
        {
            _imageStorage = imageStorage ?? throw new System.ArgumentNullException(nameof(imageStorage));
            _postsRepository = postsRepository ?? throw new System.ArgumentNullException(nameof(postsRepository));
        }

        public IActionResult Index() => View();

        [HttpGet("posts/create")]
        public IActionResult CreatePost() => View();

        [ValidateAntiForgeryToken]
        [HttpPost("posts/create")]
        public async Task<IActionResult> CreatePost(NewPostRequestModel requestModel)
        {
            if(ModelState.IsValid)
            {
                var currentUser = GetCurrentUser();

                var command = new CreatePostCommand(requestModel.Title,
                    requestModel.Abstract,
                    requestModel.Content,
                    HttpContext.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    currentUser, 
                    Enumerable.Empty<string>().ToList().AsReadOnly(),
                    true);

                await _postsRepository.CreatePost(command);

                return RedirectToAction("Index", "Home");
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
                var originalFileName = firstFile.FileName;
                var extension = Path.GetExtension(originalFileName);
                var fileName = string.Concat(DateTime.UtcNow.ToString("yyyyMMddHHmmss"), 
                    "-", 
                    Guid.NewGuid().ToString("D"),
                    "-",
                    originalFileName.Substring(0, originalFileName.Length - extension.Length),
                    extension);

                var result = await _imageStorage.SaveImage(stream, fileName);

                return Json(new 
                { 
                    ImageUrl = result.Url
                });
            }
        }

        private CreatePostCommand.User GetCurrentUser()
        {
            var id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var name = User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var surname = User.Claims.First(x => x.Type == ClaimTypes.Surname).Value;

            return new CreatePostCommand.User
            {
                Id = id,
                Name = $"{name} {surname}"
            };
        }
    }
}
