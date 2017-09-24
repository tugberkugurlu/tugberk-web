using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Tugberk.Persistance.SqlServer
{
    public abstract class BaseSlugEntity 
    {
        [Key]
        public string Path { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        [Required]
        public DateTime CreatedOnUtc { get; set; }

        [Required]
        public IdentityUser CreatedBy { get; set; }
    }
}
