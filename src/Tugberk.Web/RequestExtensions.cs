using Microsoft.AspNetCore.Http;

namespace Tugberk.Web
{
    public static class RequestExtensions 
    {
        public static string GetHostWithSchema(this HttpRequest request) 
        {
            var schema = request.IsHttps ? "https" : "http";
            return $"{schema}://{request.Host.Value}".TrimEnd('/');
        }
    }
}
