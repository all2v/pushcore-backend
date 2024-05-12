using System.Reflection.Emit;
using Astrum.News.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.News;

public class NewsDbContext : BaseDbContext
{
    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
    {
    }

    //To add migration with pmc:
    //Add-Migration Migration -Context NewsDbContext
    //To update db:
    //Update-Database -Context NewsDbContext
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostFileAttachment> PostAttachments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Widget> Widgets { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("News");

        builder.Entity<PostFileAttachment>(b =>
        {
            b.HasKey(a => a.Id);
            b.HasOne(a => a.Post)
            .WithMany(p => p.FileAttachments)
            .HasForeignKey(a => a.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Like>(b =>
        {
            b.HasKey(a => a.Id);
            b.HasOne(a => a.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(a => a.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Comment>(b =>
        {
            b.HasKey(a => a.Id);
            b.HasOne(a => a.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(a => a.PostId)
            .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(a => a.ReplyComment)
            .WithMany(p => p.ChildComments)
            .HasForeignKey(a => a.ReplyCommentId)
            .OnDelete(DeleteBehavior.Cascade);
        });
    }
}