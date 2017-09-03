using Microsoft.AspNetCore.Mvc;

namespace Tugberk.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
