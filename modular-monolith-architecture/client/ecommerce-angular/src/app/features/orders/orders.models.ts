export interface OrderItem {
  productId: string;
  productName: string;
  quantity: number;
  unitPrice: number;
  total: number;
}

export interface Order {
  id: string;
  orderNumber: string;
  status: number | string;
  subtotal: number;
  discount: number;
  shippingCost: number;
  total: number;
  items: OrderItem[];
}

export interface PaymentResponse {
  paymentId: string;
  reference: string;
  message: string;
}
