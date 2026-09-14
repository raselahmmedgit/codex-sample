export const ORDER_STATUSES = ['Pending', 'Confirmed', 'Processing', 'Packed', 'Shipped', 'Delivered'];

export function orderStatusName(status: number | string): string {
  if (typeof status === 'number') return ['Pending', 'Confirmed', 'Processing', 'Packed', 'Shipped', 'Delivered', 'Cancelled', 'Returned', 'Refunded'][status] ?? 'Unknown';
  return status;
}
