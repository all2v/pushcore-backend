using Ardalis.GuardClauses;
using Astrum.Ordering.Events;
using Astrum.SharedLib.Domain.Entities;
using Astrum.SharedLib.Domain.Interfaces;
using Astrum.SharedLib.Domain.ValueObjects;

namespace Astrum.Ordering.Aggregates;

public class Order : AggregateRootBase<Guid>,
    IDomainEventHandler<OrderCreatedEvent>,
    IDomainEventHandler<OrderItemAddedEvent>,
    IDomainEventHandler<OrderItemQuantityUpdatedEvent>
{
    private List<OrderItem> _orderItems = new();

    public Order(string trackingNumber)
    {
        RaiseEvent(new OrderCreatedEvent(trackingNumber));
    }

    public Address Address { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems
    {
        get => _orderItems.AsReadOnly();
        private set => _orderItems = value.ToList();
    }

    public decimal TotalAmount => _orderItems.Sum(oi => oi.ProductPrice * oi.Quantity);
    public string TrackingNumber { get; private set; }

    #region IDomainEventHandler<OrderCreatedEvent> Members

    void IDomainEventHandler<OrderCreatedEvent>.Apply(OrderCreatedEvent @event)
    {
        Id = @event.AggregateId;
        TrackingNumber = @event.TrackingNumber;
    }

    #endregion

    #region IDomainEventHandler<OrderItemAddedEvent> Members

    void IDomainEventHandler<OrderItemAddedEvent>.Apply(OrderItemAddedEvent @event)
    {
        _orderItems.Add(new OrderItem(@event.ProductName, @event.ProductPrice, @event.Quantity));
    }

    #endregion

    #region IDomainEventHandler<OrderItemQuantityUpdatedEvent> Members

    void IDomainEventHandler<OrderItemQuantityUpdatedEvent>.Apply(OrderItemQuantityUpdatedEvent @event)
    {
        var orderItem = _orderItems.Find(oi => oi.Id == @event.OrderItemId);
        if (orderItem == null)
            throw new NullReferenceException($"Order item with id {@event.OrderItemId} not found in order {Id}");
        orderItem.UpdateQuantity(@event.Quantity);
    }

    #endregion

    public void AddOrderItem(string productName, decimal productPrice, int quantity)
    {
        Guard.Against.NegativeOrZero(quantity, nameof(quantity), "Order item quantity cannot be 0 or negative");
        RaiseEvent(new OrderItemAddedEvent(productName, productPrice, quantity));
    }

    public void UpdateOrderItemQuantity(Guid orderItemId, int quantity)
    {
        Guard.Against.NegativeOrZero(quantity, nameof(quantity), "Order item quantity cannot be 0 or negative");
        RaiseEvent(new OrderItemQuantityUpdatedEvent(orderItemId, quantity));
    }
}