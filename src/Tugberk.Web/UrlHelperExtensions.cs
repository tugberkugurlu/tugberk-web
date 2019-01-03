using Microsoft.AspNetCore.Mvc;

namespace Tugberk.Web
{
    public static class UrlHelperExtensions 
    {
        public static string AbsoluteAction(
            this IUrlHelper url,
            string actionName, 
            string controllerName, 
            object routeValues = null)
        {
            string scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(actionName, controllerName, routeValues, scheme);
        }
    }
}
