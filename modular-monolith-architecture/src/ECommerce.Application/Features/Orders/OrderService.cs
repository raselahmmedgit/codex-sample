using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Cart;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Orders;

public sealed record CreateOrderRequest(string OrderNumber);
public sealed record OrderItemDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal Total);
public sealed record OrderDto(Guid Id, string OrderNumber, OrderStatus Status, decimal Subtotal, decimal Discount, decimal ShippingCost, decimal Total, IReadOnlyCollection<OrderItemDto> Items);
public sealed record ChangeOrderStatusRequest(OrderStatus Status, string Reason);

public sealed class OrderService(ICartRepository cartRepository, IOrderRepository orderRepository, IProductRepository productRepository)
{
    public async Task<Result<OrderDto>> CreateAsync(Guid userId, CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByUserIdAsync(userId, cancellationToken);
        if (cart is null || cart.Items.Count == 0) return Result<OrderDto>.Failure("Cart is empty.");
        var order = new Order(userId, request.OrderNumber);
        foreach (var cartItem in cart.Items)
        {
            var product = await productRepository.GetByIdAsync(cartItem.ProductId, cancellationToken);
            if (product is null || product.Status != ProductStatus.Active) return Result<OrderDto>.Failure("A product in the cart is unavailable.");
            order.AddItem(product.Id, product.Name, cartItem.Quantity, product.Price);
        }
        await orderRepository.AddAsync(order, cancellationToken); cart.Clear(); await orderRepository.SaveChangesAsync(cancellationToken);
        return Result<OrderDto>.Success(Map(order), "Order created successfully.");
    }

    public async Task<Result<IReadOnlyCollection<OrderDto>>> ListAsync(Guid userId, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.ListByUserAsync(userId, cancellationToken);
        return Result<IReadOnlyCollection<OrderDto>>.Success(orders.Select(Map).ToArray());
    }

    public async Task<Result<OrderDto>> GetAsync(Guid userId, Guid orderId, bool canManage, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order is null || (!canManage && order.UserId != userId)) return Result<OrderDto>.Failure("Order was not found.");
        return Result<OrderDto>.Success(Map(order));
    }

    public async Task<Result<OrderDto>> ChangeStatusAsync(Guid orderId, ChangeOrderStatusRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order is null) return Result<OrderDto>.Failure("Order was not found.");
        order.ChangeStatus(request.Status, request.Reason); await orderRepository.SaveChangesAsync(cancellationToken);
        return Result<OrderDto>.Success(Map(order), "Order status updated successfully.");
    }

    private static OrderDto Map(Order x) => new(x.Id, x.OrderNumber, x.Status, x.Subtotal, x.Discount, x.ShippingCost, x.Total, x.Items.Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.Quantity, i.UnitPrice, i.Total)).ToArray());
}
