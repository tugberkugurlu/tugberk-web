using System;
using Tugberk.Domain;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Web
{
    internal static class PostExtensions
    {
        public static string GeneratePostAbsoluteUrl(this PostReadModel post)
        {
            return String.Format(HardcodedConstants.BlogPostUriFormat, post.DefaultSlug.Path);
        }
    }
}