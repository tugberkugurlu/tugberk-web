using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Tugberk.Web.MediaStorage
{
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
}
