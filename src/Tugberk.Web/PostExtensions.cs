using System;
using Tugberk.Domain;

namespace Tugberk.Web
{
    internal static class PostExtensions
    {
        public static string GeneratePostAbsoluteUrl(this Post post)
        {
            return String.Format(HardcodedConstants.BlogPostUriFormat, post.DefaultSlug.Path);
        }
    }
}