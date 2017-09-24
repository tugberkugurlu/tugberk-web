using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Tugberk.Persistance.SqlServer
{
    public class TagEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        [Required]
        public IdentityUser CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOnUtc { get; set; }

        [Required]
        public string CreationIpAddress { get; set; }
        
        public ICollection<PostTagEntity> Posts { get; set; }
    }
}
