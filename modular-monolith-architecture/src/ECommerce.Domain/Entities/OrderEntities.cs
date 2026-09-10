using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public sealed class Order : AuditableEntity
{
    private readonly List<OrderItem> _items = [];
    private readonly List<OrderStatusHistory> _statusHistory = [];
    private Order() { }
    public Order(Guid userId, string orderNumber)
    {
        if (userId == Guid.Empty) throw new DomainException("User ID is required.");
        if (string.IsNullOrWhiteSpace(orderNumber)) throw new DomainException("Order number is required.");
        UserId = userId;
        OrderNumber = orderNumber.Trim();
        AddStatusHistory(OrderStatus.Pending, "Order created");
    }

    public Guid UserId { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal Subtotal { get; private set; }
    public decimal Discount { get; private set; }
    public decimal ShippingCost { get; private set; }
    public decimal Total => Subtotal - Discount + ShippingCost;
    public IReadOnlyCollection<OrderItem> Items => _items;
    public IReadOnlyCollection<OrderStatusHistory> StatusHistory => _statusHistory;

    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity <= 0) throw new DomainException("Order quantity must be greater than zero.");
        if (unitPrice < 0) throw new DomainException("Order item price cannot be negative.");
        _items.Add(new OrderItem(productId, productName, quantity, unitPrice));
        Subtotal += quantity * unitPrice;
    }

    public void ApplyDiscount(decimal discount)
    {
        if (discount < 0 || discount > Subtotal) throw new DomainException("Invalid order discount.");
        Discount = discount;
    }

    public void ChangeStatus(OrderStatus status, string reason)
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Cancelled or OrderStatus.Refunded)
            throw new DomainException("The order is already in a final state.");
        Status = status;
        AddStatusHistory(status, reason);
        MarkUpdated();
    }

    private void AddStatusHistory(OrderStatus status, string reason) => _statusHistory.Add(new OrderStatusHistory(status, reason));
}

public sealed class OrderItem : BaseEntity
{
    private OrderItem() { }
    internal OrderItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Total => Quantity * UnitPrice;
}

public sealed class OrderStatusHistory : BaseEntity
{
    private OrderStatusHistory() { }
    internal OrderStatusHistory(OrderStatus status, string reason)
    {
        Status = status;
        Reason = reason;
        ChangedAtUtc = DateTime.UtcNow;
    }
    public OrderStatus Status { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public DateTime ChangedAtUtc { get; private set; }
}

public sealed class Payment : AuditableEntity
{
    private Payment() { }
    public Payment(Guid orderId, string provider)
    {
        OrderId = orderId;
        Provider = provider;
    }
    public Guid OrderId { get; private set; }
    public string Provider { get; private set; } = string.Empty;
    public PaymentStatus Status { get; private set; } = PaymentStatus.Initiated;
    public void ChangeStatus(PaymentStatus status) => Status = status;
}

public sealed class PaymentTransaction : BaseEntity
{
    private PaymentTransaction() { }
    public PaymentTransaction(Guid paymentId, decimal amount, string reference)
    {
        PaymentId = paymentId;
        Amount = amount;
        Reference = reference;
    }
    public Guid PaymentId { get; private set; }
    public decimal Amount { get; private set; }
    public string Reference { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
}
