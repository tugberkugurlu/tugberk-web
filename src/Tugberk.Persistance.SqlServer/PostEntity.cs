using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Tugberk.Persistance.SqlServer
{
    public class PostEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Abstract { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public PostFormatEntity Format { get; set; }

        [Required]
        public IdentityUser CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOnUtc { get; set; }

        [Required]
        public string CreationIpAddress { get; set; }
        
        public ICollection<PostTagEntity> Tags { get; set; }
        public ICollection<PostSlugEntity> Slugs { get; set; }
        public ICollection<PostApprovalStatusActionEntity> ApprovalStatusActions { get; set; }
    }
}
