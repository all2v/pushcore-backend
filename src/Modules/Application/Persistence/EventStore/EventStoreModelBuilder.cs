using Astrum.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Application.EventStore;

public class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
{
    #region IEntityTypeConfiguration<Event> Members

    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(k => k.Id);
        builder
            .HasIndex(a => new {a.AggregateId, a.Version, a.AggregateName})
            .IsUnique();
        // .IsClustered(false); TODO eventStore to SQL Database
        builder.ToTable("Events", "EventStore");
    }

    #endregion
}

public class AggregateSnapshotEntityTypeConfiguration : IEntityTypeConfiguration<AggregateSnapshot>
{
    #region IEntityTypeConfiguration<AggregateSnapshot> Members

    public void Configure(EntityTypeBuilder<AggregateSnapshot> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id).UseIdentityColumn();

        builder.ToTable("Snapshots", "EventStore");
    }

    #endregion
}

public class BranchPointEntityTypeConfiguration : IEntityTypeConfiguration<BranchPoint>
{
    #region IEntityTypeConfiguration<BranchPoint> Members

    public void Configure(EntityTypeBuilder<BranchPoint> builder)
    {
        builder.HasKey(k => k.Id);
        builder.HasOne(bp => bp.Event).WithMany(e => e.BranchPoints).HasForeignKey(bp => bp.EventId)
            .IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(bp => bp.RetroactiveEvents).WithOne(ra => ra.BranchPoint)
            .HasForeignKey(ra => ra.BranchPointId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(bp => new {bp.Name, bp.EventId}).IsUnique();

        builder.ToTable("BranchPoints", "EventStore");
    }

    #endregion
}

public class RetroactiveEventEntityTypeConfiguration : IEntityTypeConfiguration<RetroactiveEvent>
{
    #region IEntityTypeConfiguration<RetroactiveEvent> Members

    public void Configure(EntityTypeBuilder<RetroactiveEvent> builder)
    {
        builder.HasKey(k => k.Id);

        builder.ToTable("RetroactiveEvents", "EventStore");
    }

    #endregion
}
