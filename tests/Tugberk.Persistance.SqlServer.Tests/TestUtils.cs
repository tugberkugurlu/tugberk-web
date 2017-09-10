using System;
using System.Linq;

namespace Tugberk.Persistance.SqlServer.Tests
{
    public static class TestUtils
    {
        private static readonly Random Random = new Random();

        /// <remarks>
        /// See http://stackoverflow.com/a/1344242/463785.
        /// </remarks>
        public static string RandomString(int length = 20)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
