using Astrum.Ordering.Aggregates;
using Astrum.Ordering.Events;
using Astrum.Ordering.Repositories;
using Astrum.SharedLib.Application.Contracts.Infrastructure.EventStore;
using Astrum.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Astrum.Core.Tests.Application.Repositories;

public class OrderRepositoryTests : TestBase
{
    private readonly IEventStore _eventStore;
    private readonly IOrderRepository _orderRepository;

    public OrderRepositoryTests()
    {
        _orderRepository = ServiceProvider.GetService<IOrderRepository>();
        _eventStore = ServiceProvider.GetService<IEventStore>();
    }

    [Fact]
    public async Task SaveToEventStore_NewOrder_OrderCreatedEvent_ExistsInStore()
    {
        var order = new Order("1234");
        await _orderRepository.SaveToEventStoreAsync(order);

        var events = await _eventStore.LoadAsync<Guid>(order.Id.ToString(), nameof(Order), 0, 1);

        Assert.NotEmpty(events);
        Assert.IsType<OrderCreatedEvent>(events.Single());
    }

    [Fact]
    public async Task SaveToEventStore_NewOrder_OrderCreatedEvent_TrackingNumber_SameAsOrder()
    {
        var trackingNumber = "1234";
        var order = new Order(trackingNumber);
        await _orderRepository.SaveToEventStoreAsync(order);

        var events = await _eventStore.LoadAsync<Guid>(order.Id.ToString(), nameof(Order), 0, 1);

        Assert.Equal(trackingNumber, ((OrderCreatedEvent)events.Single()).TrackingNumber);
    }

    [Fact]
    public async Task SaveToEventStore_AddOrderItem_OrderItemAddedEvent_ExistsInStore()
    {
        var order = new Order("1234");
        order.AddOrderItem("Product name", 100, 2);
        await _orderRepository.SaveToEventStoreAsync(order);

        var events = await _eventStore.LoadAsync<Guid>(order.Id.ToString(), nameof(Order), 0, int.MaxValue);

        Assert.Contains(events, e => e is OrderItemAddedEvent);
    }

    [Fact]
    public async Task SaveToEventStore_AddOrderItem_OrderItemAddedEvent_OrderItemDetails_SameAsOrderItem()
    {
        var productName = "Product name";
        decimal price = 100;
        var quantity = 2;
        var order = new Order("1234");
        order.AddOrderItem(productName, price, quantity);
        await _orderRepository.SaveToEventStoreAsync(order);

        var events = await _eventStore.LoadAsync<Guid>(order.Id.ToString(), nameof(Order), 0, int.MaxValue);

        var orderItemAddedEvent = (OrderItemAddedEvent)events.Single(e => e is OrderItemAddedEvent);

        Assert.Equal(productName, orderItemAddedEvent.ProductName);
        Assert.Equal(price, orderItemAddedEvent.ProductPrice);
        Assert.Equal(quantity, orderItemAddedEvent.Quantity);
    }
}