using System;
using System.Collections.Generic;
using Astrum.Application.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Application.Entities;

public class BranchPoint : BaseEntity<int>
{
    /// <summary>
    ///     Branch point indicative name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     FK for the Event Entity
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    ///     The type of the branch point
    /// </summary>
    public BranchPointTypeEnum Type { get; set; }

    /// <summary>
    ///     Navigation property of the event
    /// </summary>
    public virtual Event Event { get; set; }

    public virtual ICollection<RetroactiveEvent> RetroactiveEvents { get; set; }
}