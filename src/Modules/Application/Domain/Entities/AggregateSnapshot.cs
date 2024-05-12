using System;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Application.Entities;

public class AggregateSnapshot : BaseEntity<int>
{
    public string AggregateId { get; set; }
    public string AggregateName { get; set; }
    public int LastAggregateVersion { get; set; }
    public Guid LastEventId { get; set; }
    public string Data { get; set; }
}