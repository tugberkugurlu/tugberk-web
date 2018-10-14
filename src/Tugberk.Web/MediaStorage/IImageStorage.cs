using System.IO;
using System.Threading.Tasks;

namespace Tugberk.Web.MediaStorage
{
    public interface IImageStorage 
    {
        Task<ImageSaveResult> SaveImage(Stream content, string nameWithExtension);
    }
}
