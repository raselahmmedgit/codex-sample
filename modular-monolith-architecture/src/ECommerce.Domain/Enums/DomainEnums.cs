namespace ECommerce.Domain.Enums;

public enum ProductStatus { Draft, Active, Inactive, OutOfStock, Archived }
public enum OrderStatus { Pending, Confirmed, Processing, Packed, Shipped, Delivered, Cancelled, Returned, Refunded }
public enum PaymentStatus { Initiated, Pending, Success, Failed, Cancelled, Refunded }
public enum CouponType { Percentage, FixedAmount }
public enum ReviewModerationStatus { Pending, Approved, Rejected }
public enum InventoryTransactionType { StockIn, StockOut, Reservation, Release, Adjustment }
