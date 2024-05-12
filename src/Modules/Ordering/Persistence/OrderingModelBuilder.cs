using Astrum.Ordering.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Ordering;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    #region IEntityTypeConfiguration<Order> Members

    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Ignore(o => o.TotalAmount);
        
        builder
            .OwnsOne(o => o.Address);

        builder
            .HasMany(o => o.OrderItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Orders", "Ordering");
    }

    #endregion
}

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    #region IEntityTypeConfiguration<OrderItemEntity> Members

    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder
            .Property(oi => oi.Quantity)
            .IsRequired();
        builder
            .Property(oi => oi.ProductName)
            .IsRequired();
        builder
            .Property(oi => oi.ProductPrice)
            .IsRequired();

        builder.ToTable("OrderItems", "Ordering");
    }

    #endregion
}