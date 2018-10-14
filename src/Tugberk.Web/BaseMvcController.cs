using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tugberk.Web
{
    public abstract class BaseMvcController : Controller
    {
        public Task<IActionResult> Index() => Task.FromResult<IActionResult>(View());
    }
}
