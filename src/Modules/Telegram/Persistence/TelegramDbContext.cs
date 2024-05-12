using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Persistence.DbContexts;
using Astrum.Telegram.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Telegram.Persistence
{
    public class TelegramDbContext : BaseDbContext
    {
        public TelegramDbContext(DbContextOptions<TelegramDbContext> options) : base(options)
        {
        }

        //To add migration with pmc:
        //Add-Migration Migration -Context TelegramDbContext
        //To update db:
        //Update-Database -Context TelegramDbContext
        public DbSet<TelegramChat> Chats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Telegram");
        }
    }
}
