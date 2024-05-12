using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Persistence.DbContexts;
using Astrum.TrackerProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Astrum.TrackerProject.Persistance
{
    public class TrackerProjectDbContext : BaseDbContext
    {
        public TrackerProjectDbContext(DbContextOptions<TrackerProjectDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssueComment> IssueComments { get; set; }
        public DbSet<Domain.Aggregates.Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<ExternalUser> ExternalUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("TrackerProject");
        }
    }
}
