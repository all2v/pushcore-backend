using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.Inventory.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Inventory.Persistence
{
    public class InventoryDbContextInitializer : IDbContextInitializer
    {
        private readonly InventoryDbContext _dbContext;
        public InventoryDbContextInitializer(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region IDbContextInitializer Members

        public async Task Migrate(CancellationToken cancellationToken = default)
        {
            await _dbContext.Database.MigrateAsync(cancellationToken);
        }

        public async Task Seed(CancellationToken cancellationToken = default)
        {
            AddDefaultTemplate();
        }

        #endregion

        private void AddDefaultTemplate()
        {
            var templates = _dbContext.Templates;
            if (!templates.Any(template => template.Title == "Другое"))
            {
                templates.Add(
                    new Template
                    {
                        Title = "Другое",
                        Characteristics = new List<Characteristic>()
                    }
                );
            }
            _dbContext.SaveChanges();
        }
    }
}
