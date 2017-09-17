using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Domain;
using Tugberk.Domain.Commands;
using Tugberk.Persistance.Abstractions;
using Tugberk.Web.Models;

namespace Tugberk.Web.Controllers
{
    [Authorize]
    [Route("portal")]
    public class PortalController : Controller
    {
        private readonly IImageStorage _imageStorage;
        private readonly IPostsStore _postsStore;

        public PortalController(IImageStorage imageStorage, IPostsStore postsStore)
        {
            _imageStorage = imageStorage ?? throw new System.ArgumentNullException(nameof(imageStorage));
            _postsStore = postsStore ?? throw new System.ArgumentNullException(nameof(postsStore));
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
                User currentUser = GetCurrentUser();

                var command = new NewPostCommand(requestModel.Title,
                    requestModel.Abstract,
                    requestModel.Content,
                    PostFormat.Html,
                    HttpContext.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    currentUser,
                    Enumerable.Empty<string>().ToList().AsReadOnly());

                await _postsStore.CreatePost(command);

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
                var result = await _imageStorage.SaveImage(stream, firstFile.FileName);

                return Json(new 
                { 
                    ImageUrl = result.Url
                });
            }
        }

        private User GetCurrentUser()
        {
            var id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var name = User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var surname = User.Claims.First(x => x.Type == ClaimTypes.Surname).Value;

            return new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = $"{name} {surname}"
            };
        }
    }
}
