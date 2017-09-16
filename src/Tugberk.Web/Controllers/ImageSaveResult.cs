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
}
