using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Tugberk.Domain.Queries;
using WilderMinds.RssSyndication;

namespace Tugberk.Web.Controllers
{
    [Route("feeds")]
    public class FeedsController : Controller
    {
        const string MainRssFeedCacheKey = "main-rss";
        private readonly ILatestApprovedPostsQuery _latestApprovedPostsQuery;
        private readonly IMemoryCache _cache;
        private readonly ILogger<FeedsController> _logger;

        public FeedsController(ILatestApprovedPostsQuery latestApprovedPostsQuery, IMemoryCache cache, ILogger<FeedsController> logger)
        {
            _latestApprovedPostsQuery = latestApprovedPostsQuery ?? throw new ArgumentNullException(nameof(latestApprovedPostsQuery));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("rss")]
        [ResponseCache(Duration = 900)]
        public async Task<IActionResult> GetMainRessFeed()
        {
            Feed feed;
            if (_cache.TryGetValue(MainRssFeedCacheKey, out var cachedFeed))
            {
                _logger.LogInformation("'{MainRssFeedCacheKey}' found in the cache, will be served from there", 
                    MainRssFeedCacheKey);

                feed = (Feed) cachedFeed;
            }
            else
            {
                _logger.LogInformation("'{MainRssFeedCacheKey}' was not found in the cache, will be served cold",
                    MainRssFeedCacheKey);

                var posts = await _latestApprovedPostsQuery.GetLatestApprovedPosts(0, 20);
                feed = new Feed
                {
                    Title = "Tugberk Ugurlu @ the Heart of Software",
                    Description = "Welcome to Technical Leader and Software Engineer Tugberk Ugurlu's home on the interwebs! Here, you can find out about Tugberk's conference talks, books and blog posts on software development techniques and practices.",
                    Link = new Uri(HardcodedConstants.BlogPostHostUrl),
                    Items = posts.Items.Select(post =>
                    {
                        var postUrl = post.GeneratePostAbsoluteUrl();
                        return new Item
                        {
                            Title = post.Title,
                            Body = post.Content,
                            Link = new Uri(postUrl),
                            Permalink = postUrl,
                            PublishDate = post.CreationRecord.RecordedOn,
                            Author = new Author
                            {
                                Name = post.CreationRecord.RecordedBy.Name
                            },
                            Categories = post.Tags.Select(x => x.Name).ToList()
                        };
                    }).ToList()
                };

                _logger.LogInformation("Main Rss feed is created, setting the cache under '{MainRssFeedCacheKey}' key",
                    MainRssFeedCacheKey);

                _cache.Set(MainRssFeedCacheKey, feed, TimeSpan.FromMinutes(30));
            }

            var rssContent = feed.Serialize();
            return Content(rssContent, "application/rss+xml", Encoding.UTF8);
        }
    }
}