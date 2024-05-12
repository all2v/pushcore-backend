using Astrum.Account.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Account.Configurations;

public class RegistrationApplicationEntityTypeConfiguration : IEntityTypeConfiguration<RegistrationApplication>
{
    #region IEntityTypeConfiguration<RegistrationApplication> Members

    public void Configure(EntityTypeBuilder<RegistrationApplication> builder)
    {
        builder.Property(x => x.Status)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    #endregion
}