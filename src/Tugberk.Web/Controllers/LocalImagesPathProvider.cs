using System.IO;

namespace Tugberk.Web.Controllers
{
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
}
