import { CartDto, WishlistDto } from '../models/commerce.models';
import { Address } from '../../features/customer/customer.models';
import { Order } from '../../features/orders/orders.models';

export const demoCart: CartDto = {
  id: 'demo-cart',
  items: [
    { id: 'demo-cart-item-1', productId: '10000000-0000-0000-0000-000000000001', quantity: 1, unitPrice: 129.99, total: 129.99 },
    { id: 'demo-cart-item-2', productId: '10000000-0000-0000-0000-000000000004', quantity: 2, unitPrice: 24.95, total: 49.90 }
  ],
  total: 179.89
};

export const demoWishlist: WishlistDto = {
  id: 'demo-wishlist',
  items: [
    { id: 'demo-wishlist-item-1', productId: '10000000-0000-0000-0000-000000000003' },
    { id: 'demo-wishlist-item-2', productId: '10000000-0000-0000-0000-000000000007' }
  ]
};

export const demoOrders: Order[] = [
  {
    id: '20000000-0000-0000-0000-000000000001', orderNumber: 'ELV-2026-0001', status: 'Delivered', subtotal: 129.99, discount: 0, shippingCost: 0, total: 129.99,
    items: [{ productId: '10000000-0000-0000-0000-000000000001', productName: 'Elevate Wireless Headphones', quantity: 1, unitPrice: 129.99, total: 129.99 }]
  },
  {
    id: '20000000-0000-0000-0000-000000000002', orderNumber: 'ELV-2026-0002', status: 'Confirmed', subtotal: 114.95, discount: 10, shippingCost: 0, total: 104.95,
    items: [{ productId: '10000000-0000-0000-0000-000000000004', productName: 'Stoneware Coffee Mug', quantity: 2, unitPrice: 24.95, total: 49.90 }, { productId: '10000000-0000-0000-0000-000000000005', productName: 'Cloud Notes Notebook', quantity: 1, unitPrice: 18.75, total: 18.75 }]
  }
];

export const demoAddresses: Address[] = [
  { id: 'demo-address-1', line1: '42 Lake View Road', line2: 'Apartment 4B', city: 'Dhaka', state: 'Dhaka', postalCode: '1212', country: 'Bangladesh', isDefault: true },
  { id: 'demo-address-2', line1: '18 Station Road', line2: null, city: 'Chattogram', state: 'Chattogram', postalCode: '4000', country: 'Bangladesh', isDefault: false }
];
