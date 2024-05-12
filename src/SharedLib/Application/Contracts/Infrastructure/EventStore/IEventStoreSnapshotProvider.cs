using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.SharedLib.Application.Contracts.Infrastructure.EventStore;

/// <summary>
///     Interface for fetching/saving snapshot aggregates
/// </summary>
public interface IEventStoreSnapshotProvider
{
    Task<T> GetAggregateFromSnapshotAsync<T, TAggregateId>(TAggregateId aggregateId, string aggregateName)
        where T : class, IAggregateRoot<TAggregateId>;

    Task SaveSnapshotAsync<T, TId>(T aggregate, Guid lastEventId) where T : class, IAggregateRoot<TId>;
}