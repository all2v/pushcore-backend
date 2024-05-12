using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Account.Configurations;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Astrum.Account.Aggregates.Account>
{
    #region IEntityTypeConfiguration<Account> Members

    public void Configure(EntityTypeBuilder<Astrum.Account.Aggregates.Account> builder)
    {
    }

    #endregion
}