using System;

namespace Tugberk.Domain
{
    public class Slug
    {
        public string Path { get; set; }
        public bool IsDefault { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
