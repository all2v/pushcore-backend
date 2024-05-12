using System;
using System.Collections.Generic;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Application.Entities;

public class Event : BaseEntity<Guid>
{
    /// <summary>
    ///     Name of the aggregate. This is basically the class name or entity name
    /// </summary>
    public string AggregateName { get; set; }

    /// <summary>
    ///     The Id of the aggregate this event is for.
    ///     Using this field, events for the given aggregate can be filtered
    /// </summary>
    public string AggregateId { get; set; }

    /// <summary>
    ///     Date this event was created
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///     The actual name of the event
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Full assembly type name for this event
    ///     Used for recreation purposes
    /// </summary>
    public string AssemblyTypeName { get; set; }

    /// <summary>
    ///     Payload of the event
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    ///     Event version.
    ///     Increments each time for given aggregate. Used in optimistic concurrency check.
    /// </summary>
    public int Version { get; set; }

    public virtual ICollection<BranchPoint> BranchPoints { get; set; }
}