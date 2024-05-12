using Astrum.SharedLib.Persistence.Models.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.SharedLib.Persistence.Configurations;

/// <summary>
/// </summary>
public class AuditHistoryEntityTypeConfiguration : IEntityTypeConfiguration<AuditHistory>
{
    #region IEntityTypeConfiguration<AuditHistory> Members

    public void Configure(EntityTypeBuilder<AuditHistory> builder)
    {
        builder.Property(c => c.Id)
            .UseIdentityColumn(); //TODO: Possibly change this to avoid integer overflow, or cleanup every once in a while
        builder.Property(c => c.RowId)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(c => c.TableName)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(c => c.Changed);
        builder.Property(c => c.Username)
            .HasMaxLength(128);
        // This MSSQL only
        //b.Property(c => c.Created).HasDefaultValueSql("getdate()");
        builder.Ignore(t => t.AutoHistoryDetails);
        builder.ToTable("AuditHistory");
    }

    #endregion
}