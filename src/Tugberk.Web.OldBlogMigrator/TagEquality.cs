using System.Collections.Generic;
using Bloggy.Domain.Entities;

namespace Tugberk.Web.OldBlogMigrator
{
    internal class TagEquality : IEqualityComparer<Tag>
    {
        public bool Equals(Tag x, Tag y) => x.Name.Equals(y.Name);
        public int GetHashCode(Tag obj) => obj.Name.GetHashCode();
    }
}