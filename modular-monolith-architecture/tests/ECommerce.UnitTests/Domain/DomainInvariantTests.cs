using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.UnitTests.Domain;

public sealed class DomainInvariantTests
{
    [Fact]
    public void Product_ShouldRejectNegativePrice()
    {
        var action = () => new Product("SKU-001", "Laptop", -1m);
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Cart_ShouldMergeSameProductAndUseServerPrice()
    {
        var cart = new Cart(Guid.NewGuid());
        cart.AddItem(Guid.NewGuid(), 2, 100m);
        cart.AddItem(cart.Items.Single().ProductId, 1, 120m);

        var item = Assert.Single(cart.Items);
        Assert.Equal(3, item.Quantity);
        Assert.Equal(120m, item.UnitPrice);
        Assert.Equal(360m, item.Total);
    }

    [Fact]
    public void Inventory_ShouldPreventReservationBeyondAvailableQuantity()
    {
        var inventory = new Inventory(Guid.NewGuid(), 5);
        inventory.Reserve(3);

        Assert.Throws<DomainException>(() => inventory.Reserve(3));
        Assert.Equal(2, inventory.AvailableQuantity);
    }

    [Fact]
    public void Order_ShouldRecordStatusHistory()
    {
        var order = new Order(Guid.NewGuid(), "ORD-001");
        order.ChangeStatus(OrderStatus.Confirmed, "Payment approved");

        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.Equal(2, order.StatusHistory.Count);
    }

    [Fact]
    public void Review_ShouldRejectRatingOutsideOneToFive()
    {
        var action = () => new Review(Guid.NewGuid(), Guid.NewGuid(), 6, "Invalid rating");
        Assert.Throws<DomainException>(action);
    }
}
