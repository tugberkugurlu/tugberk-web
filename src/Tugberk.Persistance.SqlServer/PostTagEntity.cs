using System;
using System.ComponentModel.DataAnnotations;

namespace Tugberk.Persistance.SqlServer
{
    public class PostTagEntity
    {
        [Required]
        public Guid PostId { get; set; }

        [Required]
        public PostEntity Post { get; set; }

        [Required]
        public string TagName { get; set; }

        [Required]
        public TagEntity Tag { get; set; }
    }
}
