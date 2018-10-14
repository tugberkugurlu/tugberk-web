using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.StaticFiles;
using Tugberk.Web.MediaStorage;

namespace Tugberk.Web.Controllers
{
    [Route("local")]
    public class LocalController : Controller 
    {

#if DEBUG
    private const bool IsDebug = true;
#else
    private const bool IsDebug = false;
#endif 

        [HttpGet("images/{name}")]
        public IActionResult Image(string name)
        {
            var imagePath = LocalImagesPathProvider.GetImagePath(name);
            if(System.IO.File.Exists(imagePath)) 
            {
                if(new FileExtensionContentTypeProvider().TryGetContentType(name, out var contentType)) 
                {
                    return File(System.IO.File.OpenRead(imagePath), contentType);
                }
            }

            return NotFound();
        }

        public override void OnActionExecuting(ActionExecutingContext context) 
        {
            if(!IsDebug || !context.HttpContext.Request.IsLocal())
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}
