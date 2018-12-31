using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Tugberk.Web.MediaStorage
{
    public class AzureBlobStorageImageStorage : IImageStorage
    {
        private const string ContainerReferenceName = "bloggyimages";
        private static readonly ReadOnlyDictionary<string, string> MimeTypes = new ReadOnlyDictionary<string, string>(
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".gif", "image/gif" }
            });

        private readonly CloudBlobClient _blobClient;

        public AzureBlobStorageImageStorage(CloudBlobClient blobClient)
        {
            _blobClient = blobClient ?? throw new ArgumentNullException(nameof(blobClient));
        }

        public async Task<ImageSaveResult> SaveImage(Stream content, string nameWithExtension)
        {
            var extension = Path.GetExtension(nameWithExtension);
            if(!MimeTypes.TryGetValue(extension, out var contentType)) 
            {
                throw new InvalidOperationException($"'{nameWithExtension}' is not allowed as a file to upload due to its unsupported content type. Supported content types are: '{string.Join(";", MimeTypes.Keys)}'");
            }

            var container = _blobClient.GetContainerReference(ContainerReferenceName);
            if((await container.CreateIfNotExistsAsync())) 
            {
                await container.SetPermissionsAsync(new BlobContainerPermissions 
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            var blob = container.GetBlockBlobReference(nameWithExtension);
            blob.Properties.ContentType = contentType;

            await blob.UploadFromStreamAsync(content);

            return new ImageSaveResult(blob.Uri.AbsoluteUri);
        }
    }
}
