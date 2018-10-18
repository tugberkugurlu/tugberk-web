using System.Collections.Generic;

namespace Tugberk.Domain
{
    public class Tag
    {
        public string Name { get; set; }
        public IReadOnlyCollection<Slug> Slugs { get; set; }
        public int PostsCount { get; set; }
    }
}
