using System;

namespace Tugberk.Web.Models
{
    /// <seealso href="https://developer.twitter.com/en/docs/tweets/optimize-with-cards/guides/getting-started.html" />
    public class TwitterCardContent
    {
        public TwitterCardContent(string title, string contentAbsoluteUrl, string description, string imageAbsoluteUrl, string siteTwitterHandle, string authorTwitterHandle)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            ContentAbsoluteUrl = contentAbsoluteUrl ?? throw new ArgumentNullException(nameof(contentAbsoluteUrl));
            Description = description;
            ImageAbsoluteUrl = imageAbsoluteUrl;
            SiteTwitterHandle = siteTwitterHandle;
            AuthorTwitterHandle = authorTwitterHandle;
        }

        public string Title { get; }
        public string ContentAbsoluteUrl { get; }
        public string Description { get; }
        public string ImageAbsoluteUrl { get; }
        public string SiteTwitterHandle { get; }
        public string AuthorTwitterHandle { get; }
    }
}