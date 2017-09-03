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
        public DateTime CreatedOnUtc { get; set; }

        [Required]
        public string CreationIpAddress { get; set; }
        public ICollection<PostTagEntity> Posts { get; set; }
    }

    public class PostTagEntity
    {
        public Guid PostId { get; set; }
        public PostEntity Post { get; set; }

        public string TagName { get; set; }
        public TagEntity Tag { get; set; }
    }

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

        public PostFormatEntity Format { get; set; }
        public IdentityUser CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        [Required]
        public string CreationIpAddress { get; set; }
        public ICollection<PostTagEntity> Tags { get; set; }
    }
}
