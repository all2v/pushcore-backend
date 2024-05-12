using Astrum.Projects.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Projects.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    #region IEntityTypeConfiguration<Product> Members

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .HasOne(e => e.Customer)
            .WithMany()
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasMany(e => e.Projects)
            .WithOne()
            .HasForeignKey(e=>e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .Property(e => e.Index)
            .ValueGeneratedOnAdd();
    }

    #endregion
}