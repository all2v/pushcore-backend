using Astrum.Ordering.Aggregates;
using Xunit;

namespace Astrum.Core.Tests.Domain;

public class OrderTests
{
    [Fact]
    public void Order_Create_SetsTrackingNumber()
    {
        var trackingNumber = "1234";
        var order = new Order(trackingNumber);

        Assert.Equal(trackingNumber, order.TrackingNumber);
    }

    [Fact]
    public void Order_AddOrderItem_OrderItems_NotEmpty()
    {
        var order = new Order("");
        order.AddOrderItem("", 1, 1);

        Assert.NotEmpty(order.OrderItems);
    }

    [Fact]
    public void Order_AddOrderItem_AddsOrderItemWithSpecifiedData()
    {
        var productName = "product name";
        decimal productPrice = 10;
        var quantity = 1;

        var order = new Order("");
        order.AddOrderItem(productName, productPrice, quantity);
        var orderItem = order.OrderItems.Single();

        Assert.Equal(productName, orderItem.ProductName);
        Assert.Equal(productPrice, orderItem.ProductPrice);
        Assert.Equal(quantity, orderItem.Quantity);
    }

    [Fact]
    public void Order_AddOrderItem_OnZeroQuantity_ThrowsArgumentException()
    {
        var order = new Order("");
        Assert.Throws<ArgumentException>(() => order.AddOrderItem("", 1, 0));
    }

    [Fact]
    public void Order_AddOrderItem_OnNegativeQuantity_ThrowsArgumentException()
    {
        var order = new Order("");
        Assert.Throws<ArgumentException>(() => order.AddOrderItem("", -1, 0));
    }

    [Fact]
    public void Order_UpdateOrderItemQuantity_Quantity_EqualToSet()
    {
        var previousQuantity = 1;
        var updatedQuantity = 2;
        var order = new Order("");
        order.AddOrderItem("productName", 10, previousQuantity);
        var orderItemId = order.OrderItems.Single().Id;

        order.UpdateOrderItemQuantity(orderItemId, updatedQuantity);

        Assert.Equal(updatedQuantity, order.OrderItems.Single().Quantity);
    }

    [Fact]
    public void OrderItem_UpdateOrderItemQuantity_OrderItemNotFound_ThrowsNullReferenceException()
    {
        var order = new Order("");
        Assert.Throws<NullReferenceException>(() => order.UpdateOrderItemQuantity(Guid.NewGuid(), 1));
    }

    [Fact]
    public void Order_UpdateOrderItemQuantity_QuantityNegative_ThrowsArgumentException()
    {
        var order = new Order("");
        order.AddOrderItem("productName", 10, 1);
        var orderItemId = order.OrderItems.Single().Id;
        Assert.Throws<ArgumentException>(() => order.UpdateOrderItemQuantity(Guid.NewGuid(), -1));
    }

    [Fact]
    public void Order_UpdateOrderItemQuantity_QuantityZero_ThrowsArgumentException()
    {
        var order = new Order("");
        order.AddOrderItem("productName", 10, 1);
        var orderItemId = order.OrderItems.Single().Id;
        Assert.Throws<ArgumentException>(() => order.UpdateOrderItemQuantity(Guid.NewGuid(), 0));
    }
}