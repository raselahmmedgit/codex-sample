export interface Review {
  id: string;
  productId: string;
  userId: string;
  rating: number;
  comment: string;
  moderationStatus: number | string;
}
