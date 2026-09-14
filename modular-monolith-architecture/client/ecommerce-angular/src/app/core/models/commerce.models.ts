export interface CartItem {
  id: string;
  productId: string;
  quantity: number;
  unitPrice: number;
  total: number;
}

export interface CartDto {
  id: string;
  items: CartItem[];
  total: number;
}

export interface WishlistDto {
  id: string;
  items: Array<{ id: string; productId: string }>;
}
