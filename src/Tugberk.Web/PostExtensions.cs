using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Web
{
    internal static class PostExtensions
    {
        public static string GeneratePostAbsoluteUrl(this PostReadModel post)
        {
            return string.Format(HardcodedConstants.BlogPostUriFormat, post.DefaultSlug.Path);
        }
    }
}