using System;

namespace Tugberk.Persistance.SqlServer
{
    public class PostTagEntity
    {
        public Guid PostId { get; set; }
        public PostEntity Post { get; set; }

        public string TagName { get; set; }
        public TagEntity Tag { get; set; }
    }
}
