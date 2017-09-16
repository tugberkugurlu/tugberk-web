using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Tugberk.Web.Controllers
{
    public class ImageSaveResult 
    {
        public ImageSaveResult(string url)
        {
            Url = url ?? throw new System.ArgumentNullException(nameof(url));
        }

        public string Url { get; }
    }

    public interface IImageStorage 
    {
        Task<ImageSaveResult> SaveImage(Stream content, string nameWithExtension);
    }

    public static class LocalImagesPathProvider 
    {
        private const string ImagesFolderName = "Tugberk_Web_Images";
        private static readonly string _imagesFolder;

        static LocalImagesPathProvider() 
        {
            var tempDir = Path.GetTempPath();
            _imagesFolder = Path.Combine(tempDir, ImagesFolderName);
            EnsureExists(_imagesFolder);
        }

        public static string GetImagePath(string nameWithExtension) => 
            Path.Combine(_imagesFolder, nameWithExtension);

        public static string GetRandomImagePath(string baseNameWithExtension)
        {
            var uniqueIdentifier = Path.GetRandomFileName();
            var fileName = $"{uniqueIdentifier}_{baseNameWithExtension}";
            return Path.Combine(_imagesFolder, fileName);
        }

        private static void EnsureExists(string imagesFolder)
        {
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }
        }
    }

    public class LocalImageStorage : IImageStorage
    {
        private const string UrlPrefix = "http://localhost:5000/local/images/";
        private readonly ILogger<LocalImageStorage> _logger;

        public LocalImageStorage(ILogger<LocalImageStorage> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task<ImageSaveResult> SaveImage(Stream content, string nameWithExtension)
        {
            var imagePath = LocalImagesPathProvider.GetRandomImagePath(nameWithExtension);
            var fileName = Path.GetFileName(imagePath);

            _logger.LogInformation("Saving {baseFileName} under {imagePath}", 
                nameWithExtension, imagePath);

            using(var fileStream = File.Open(imagePath, FileMode.CreateNew))
            {
                await content.CopyToAsync(fileStream);
            }

            return new ImageSaveResult($"{UrlPrefix}{fileName}");
        }
    }

    [Route("portal")]
    public class PortalController : Controller
    {
        private readonly IImageStorage _imageStorage;

        public PortalController(IImageStorage imageStorage)
        {
            _imageStorage = imageStorage ?? throw new System.ArgumentNullException(nameof(imageStorage));
        }

        public IActionResult Index() => View();

        [HttpGet("posts")]
        public IActionResult CreatePost() => View();

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
