using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Appeal.Configurations;

public class AppealEntityTypeConfiguration : IEntityTypeConfiguration<Aggregates.Appeal>
{
    #region IEntityTypeConfiguration<Appeal> Members

    public void Configure(EntityTypeBuilder<Aggregates.Appeal> builder)
    {
        builder.Metadata.FindNavigation(nameof(Aggregates.Appeal.AppealCategories))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    #endregion
}