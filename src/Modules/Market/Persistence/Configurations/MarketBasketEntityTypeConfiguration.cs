using Astrum.Market.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Market.Configurations;

public class MarketBasketEntityTypeConfiguration : IEntityTypeConfiguration<MarketBasket>
{
    #region IEntityTypeConfiguration<MarketBasket> Members

    public void Configure(EntityTypeBuilder<MarketBasket> builder)
    {
        builder.Metadata.FindNavigation(nameof(MarketBasket.Products))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    #endregion
}