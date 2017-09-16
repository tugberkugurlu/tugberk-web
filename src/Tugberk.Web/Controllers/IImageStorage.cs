using System.IO;
using System.Threading.Tasks;

namespace Tugberk.Web.Controllers
{
    public interface IImageStorage 
    {
        Task<ImageSaveResult> SaveImage(Stream content, string nameWithExtension);
    }
}
