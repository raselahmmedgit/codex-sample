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

    [Fact]
    public void Order_ShouldCalculateSubtotalDiscountAndTotal()
    {
        var order = new Order(Guid.NewGuid(), "ORD-002");
        order.AddItem(Guid.NewGuid(), "Keyboard", 2, 50m);
        order.AddItem(Guid.NewGuid(), "Mouse", 1, 25m);
        order.ApplyDiscount(20m);

        Assert.Equal(125m, order.Subtotal);
        Assert.Equal(20m, order.Discount);
        Assert.Equal(105m, order.Total);
    }

    [Fact]
    public void Coupon_ShouldNormalizeCodeAndCalculatePercentageDiscount()
    {
        var coupon = new Coupon(" save10 ", CouponType.Percentage, 10m,
            DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(10));

        Assert.Equal("SAVE10", coupon.Code);
        Assert.Equal(10m, coupon.CalculateDiscount(100m));
    }

    [Fact]
    public void DeactivatedCoupon_ShouldNotProvideDiscount()
    {
        var coupon = new Coupon("SAVE10", CouponType.FixedAmount, 10m,
            DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(10));
        coupon.Deactivate();

        Assert.False(coupon.IsValid(DateTime.UtcNow, 100m));
        Assert.Equal(0m, coupon.CalculateDiscount(100m));
    }

    [Fact]
    public void Inventory_ShouldReleaseReservedQuantity()
    {
        var inventory = new Inventory(Guid.NewGuid(), 10);
        inventory.Reserve(4);
        inventory.Release(2);

        Assert.Equal(2, inventory.ReservedQuantity);
        Assert.Equal(8, inventory.AvailableQuantity);
    }
}
