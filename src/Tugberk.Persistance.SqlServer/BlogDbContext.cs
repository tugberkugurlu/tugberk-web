using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Tugberk.Persistance.SqlServer
{
    public class BlogDbContext : IdentityDbContext<IdentityUser>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) 
        {
        }
        
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<TagEntity> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TagEntity>().HasKey(bc => bc.Name);
            builder.Entity<PostTagEntity>()
                .HasKey(t => new { t.PostId, t.TagName });

            builder.Entity<PostTagEntity>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.PostId);

            builder.Entity<PostTagEntity>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.Posts)
                .HasForeignKey(pt => pt.TagName);

            base.OnModelCreating(builder);
        }
    }
}
