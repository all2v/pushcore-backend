using Astrum.Inventory.Domain.Aggregates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Inventory.Persistence.Configurations
{
    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        #region IEntityTypeConfiguration<InventoryItem> Members
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder
            .HasOne(e => e.Template)
            .WithMany()
            .HasForeignKey(e => e.TemplateId)
            .OnDelete(DeleteBehavior.SetNull);
            builder
            .Property(e => e.Index)
            .ValueGeneratedOnAdd();
        }
        #endregion
    }
}
