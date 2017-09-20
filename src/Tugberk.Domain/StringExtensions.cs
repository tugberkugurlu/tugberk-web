using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Tugberk.Domain
{
    public static class StringExtensions
    {
        /// <remarks>
        /// Does not allow leading or trailing hypens or whitespace.
        /// </remarks>
        public static readonly Regex SlugRegex = new Regex(@"(^[a-z0-9])([a-z0-9_-]+)*([a-z0-9])$", RegexOptions.Compiled);

        public static string ToSlug(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (SlugRegex.IsMatch(value))
            {
                return value;
            }

            return GenerateSlug(value);
        }

        /// <summary>
        /// Credit for this method goes to http://stackoverflow.com/questions/2920744/url-slugify-alrogithm-in-cs and
        /// https://github.com/benfoster/Fabrik.Common/blob/dev/src/Fabrik.Common/StringExtensions.cs.
        /// </summary>
        private static string GenerateSlug(string phrase)
        {
            string result = RemoveAccent(phrase).ToLowerInvariant();
            result = result.Trim('-', '.');
            result = result.Replace('.', '-');
            result = result.Replace("#", "-sharp");
            result = Regex.Replace(result, @"[^a-z0-9\s-]", string.Empty); // remove invalid characters
            result = Regex.Replace(result, @"\s+", " ").Trim(); // convert multiple spaces into one space

            return Regex.Replace(result, @"\s", "-"); // replace all spaces with hyphens
        }

        private static string RemoveAccent(string txt)
        {
            // broken on rc2, see: https://github.com/dotnet/corefx/issues/9158
            // byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            byte[] bytes = Encoding.UTF8.GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
