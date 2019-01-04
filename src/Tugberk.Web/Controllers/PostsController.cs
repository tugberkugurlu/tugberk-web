using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Domain;
using Tugberk.Persistance.Abstractions;
using System.Linq;
using Microsoft.Extensions.Logging;
using Tugberk.Web.Models;

namespace Tugberk.Web.Controllers
{
    public class TagViewModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public int PostsCount { get; set; }
    }

    public class TagPageViewModel
    {
        public TagPageViewModel(int currentPage, string tagSlug, Paginated<Post> paginatedPostsResult)
        {
            if (currentPage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentPage));
            }

            CurrentPage = currentPage;
            TagSlug = tagSlug ?? throw new ArgumentNullException(nameof(tagSlug));
            PaginatedPostsResult = paginatedPostsResult ?? throw new ArgumentNullException(nameof(paginatedPostsResult));
        }

        public int CurrentPage { get; }
        public string TagSlug { get; }
        public Paginated<Post> PaginatedPostsResult { get; }
    }
    
    public class MonthYearPageViewModel
    {
        public MonthYearPageViewModel(int currentPage, int month, int year, Paginated<Post> paginatedPostsResult)
        {
            if (currentPage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentPage));
            }

            CurrentPage = currentPage;
            Month = month;
            Year = year;
            PaginatedPostsResult = paginatedPostsResult ?? throw new ArgumentNullException(nameof(paginatedPostsResult));
        }

        public int CurrentPage { get; }
        public int Month { get; }
        public int Year { get; }
        public Paginated<Post> PaginatedPostsResult { get; }
    }

    public class PostsController : Controller
    {
        private readonly IPostsRepository _postsRepository;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IPostsRepository postsRepository, ILogger<PostsController> logger)
        {
            _postsRepository = postsRepository ?? throw new ArgumentNullException(nameof(postsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("archive/{slug}")]
        public async Task<IActionResult> Index(string slug)
        {
            var result = await _postsRepository.FindApprovedPostBySlug(slug);

            return result.Match<IActionResult>(
                some => 
                {
                    return some.Match<IActionResult>(
                        foundPost =>
                        {
                            ViewBag.PageTitle = foundPost.Title;
                            ViewBag.PageDescription = foundPost.Abstract;
                            ViewBag.TwitterCard = ConstructTwitterCardContent(foundPost);

                            return View(foundPost);
                        },
                        notApproved => 
                        {
                            return NotFound();
                        });
                },
                
                () => 
                {
                    return NotFound();
                });
        }

        [HttpGet("tags/{tagSlug}")]
        public async Task<IActionResult> GetByTag(string tagSlug, int page)
        {
            if (page < 0)
            {
                return NotFound();
            }

            int skip = 5 * page;
            int take = 5;

            var result = await _postsRepository.GetLatestApprovedPosts(tagSlug, skip, take);

            if (result.Items.Count == 0)
            {
                return NotFound();
            }

            return View(new TagPageViewModel(page, tagSlug, result));
        }
        
        [HttpGet("archive/{month:int:min(1):max(12)}/{year:int}")]
        public async Task<IActionResult> GetByYearMonth(int month, int year, int page)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
            {
                _logger.LogWarning("The year value '{Year}' is out of range", year);
                return NotFound();
            }
            
            if (page < 0)
            {
                return NotFound();
            }
            
            int skip = 5 * page;
            int take = 5;
            
            var result = await _postsRepository.GetLatestApprovedPosts(month, year, skip, take);

            if (result.Items.Count == 0)
            {
                return NotFound();
            }

            return View(new MonthYearPageViewModel(page, month, year, result));
        }

        private TwitterCardContent ConstructTwitterCardContent(Post post)
        {
            var postImageUrl = ExtractPostImageUrl(post);
            return new TwitterCardContent(
                post.Title,
                post.GeneratePostAbsoluteUrl(),
                !string.IsNullOrWhiteSpace(post.Abstract) ? post.Abstract : null,
                postImageUrl,
                HardcodedConstants.BlogTwitterHandle,
                HardcodedConstants.BlogTwitterHandle);
        }

        private string ExtractPostImageUrl(Post post)
        {
            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(post.Content);

                // see https://stackoverflow.com/a/4835960/463785
                var imageNodes = htmlDoc.DocumentNode.SelectNodes("//img[@src]");
                if (imageNodes != null && imageNodes.Any())
                {
                    return imageNodes.First().Attributes["src"].Value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error while extracting an image for blog post with Id '{BlogPostId}' and Title '{BlogPostTitle}'",
                    post.Id, post.Title);
            }

            return null;
        }
    }
}
