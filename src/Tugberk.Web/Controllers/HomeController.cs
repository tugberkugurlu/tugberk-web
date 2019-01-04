using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Domain.ReadSide.Queries;
using Tugberk.Web.Models;

namespace Tugberk.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILatestApprovedPostsQuery _latestApprovedPostsQuery;

        public HomeController(ILatestApprovedPostsQuery latestApprovedPostsQuery)
        {
            _latestApprovedPostsQuery = latestApprovedPostsQuery ?? throw new ArgumentNullException(nameof(latestApprovedPostsQuery));
        }

        public async Task<IActionResult> Index(int page) 
        {
            if(page < 0) 
            {
                return NotFound();
            }

            int skip = 5 * page;
            int take = 5;

            var result = await _latestApprovedPostsQuery.GetLatestApprovedPosts(skip, take);

            if (page > 0 && result.Items.Count == 0)
            {
                return NotFound();
            }

            return View(new HomePageViewModel(page, result));
        }
    }
}
