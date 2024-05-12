using Astrum.Application.Entities;
using Astrum.Ordering.Events;
using Astrum.SharedLib.Application.Contracts.Infrastructure.EventStore;
using Astrum.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Astrum.Infrastructure.Tests;

public class EventStoreTests : TestBase
{
    private readonly IEventStore _eventStore;

    public EventStoreTests()
    {
        _eventStore = ServiceProvider.GetService<IEventStore>();
    }

    [Fact]
    public async Task EventStore_SaveAsync_SavesEventToStore()
    {
        var aggregateName = "some aggregate name";
        var aggregateVersion = 1;
        await _eventStore.SaveAsync(aggregateName, aggregateVersion,
            new OrderItemAddedEvent("some product name", 100, 1));

        var eventsFromStore = EventStoreContext.Events.ToList();

        Assert.NotEmpty(eventsFromStore);
    }

    [Fact]
    public async Task EventStore_LoadAsync_AggregateNotFound_ReturnsEmptyDomainEventList()
    {
        var events = await _eventStore.LoadAsync<Guid>("aggregate root id which does not exist", "Order", 0, 1);

        Assert.Empty(events);
    }

    [Fact]
    public async Task EventStore_LoadAsync_FromVersion_Negative_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => await _eventStore.LoadAsync<Guid>("", "", -1, 0));
    }

    [Fact]
    public async Task EventStore_LoadAsync_ToVersion_Negative_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => await _eventStore.LoadAsync<Guid>("", "", 0, -1));
    }

    [Fact]
    public async Task EventStore_LoadAsync_ToVersionSmallerThanFromVersion_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => await _eventStore.LoadAsync<Guid>("", "", 6, 5));
    }

    [Fact]
    public async Task EventStore_LoadAsync_AggregateVersionNotFound_ReturnsEmptyDomainEventList()
    {
        var aggregateId = "8fd1f798-073a-4c40-b1da-f462876c8933";
        AddDummyEvent(typeof(OrderCreatedEvent), aggregateId, 0);
        var events = await _eventStore.LoadAsync<Guid>(aggregateId, "Order", 10, 11);

        Assert.Empty(events);
    }

    [Fact]
    public async Task EventStore_LoadAsync_ReturnsDomainEventsForAggregate_NotEmpty()
    {
        var aggregateId = "8fd1f798-073a-4c40-b1da-f462876c8933";
        var aggergateVersion = 0;
        AddDummyEvent(typeof(OrderCreatedEvent), aggregateId, aggergateVersion);
        var events = await _eventStore.LoadAsync<Guid>(aggregateId, "Order", aggergateVersion, 1);

        Assert.NotEmpty(events);
    }

    [Fact]
    public async Task EventStore_LoadAsync_ReturnsDomainEventsForAggregate_Events_ContainEventType()
    {
        var aggregateId = "8fd1f798-073a-4c40-b1da-f462876c8933";
        var eventType = typeof(OrderCreatedEvent);
        AddDummyEvent(eventType, aggregateId, 0);
        var events = await _eventStore.LoadAsync<Guid>(aggregateId, "Order", 0, 1);

        Assert.IsType(eventType, events.Single());
    }

    private void AddDummyEvent(Type type, string aggregateId, int version)
    {
        var eventId = Guid.NewGuid();
        EventStoreContext.Events.Add(
            new Event
            {
                Id = eventId,
                AggregateName = "Order",
                AggregateId = aggregateId,
                CreatedAt = DateTimeOffset.UtcNow,
                Name = type.Name,
                AssemblyTypeName = type.AssemblyQualifiedName,
                Data = "{\"TrackingNumber\":\"TRACKING NUMBER HERE\",\"EventId\":\"" + eventId +
                       "\",\"AggregateId\":\"" + aggregateId + "\",\"AggregateVersion\":" + version + "}",
                Version = version
            });
        EventStoreContext.SaveChanges();
    }
}