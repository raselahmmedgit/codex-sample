import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of } from 'rxjs';
import { CartDto, WishlistDto } from '../../core/models/commerce.models';
import { demoCart, demoWishlist } from '../../core/data/demo-commerce.data';

interface ApiResult<T> { succeeded: boolean; data: T | null; message: string; errors: string[]; }

@Injectable({ providedIn: 'root' })
export class CommerceActionsService {
  private readonly http = inject(HttpClient);

  getCart(): Observable<CartDto> { return this.http.get<ApiResult<CartDto>>('/api/cart').pipe(map(result => this.unwrap(result)), catchError(() => of(demoCart))); }
  updateCartItem(id: string, quantity: number): Observable<CartDto> { return this.http.put<ApiResult<CartDto>>(`/api/cart/items/${id}`, { quantity }).pipe(map(result => this.unwrap(result))); }
  removeCartItem(id: string): Observable<boolean> { return this.http.delete<ApiResult<boolean>>(`/api/cart/items/${id}`).pipe(map(result => this.unwrap(result))); }
  clearCart(): Observable<boolean> { return this.http.delete<ApiResult<boolean>>('/api/cart').pipe(map(result => this.unwrap(result))); }
  addToCart(productId: string, quantity = 1): Observable<CartDto> { return this.http.post<ApiResult<CartDto>>('/api/cart/items', { productId, quantity }).pipe(map(result => this.unwrap(result))); }

  getWishlist(): Observable<WishlistDto> { return this.http.get<ApiResult<WishlistDto>>('/api/wishlist').pipe(map(result => this.unwrap(result)), catchError(() => of(demoWishlist))); }
  removeWishlistItem(id: string): Observable<boolean> { return this.http.delete<ApiResult<boolean>>(`/api/wishlist/items/${id}`).pipe(map(result => this.unwrap(result))); }

  private unwrap<T>(result: ApiResult<T>): T {
    if (!result.succeeded || result.data === null) throw new Error(result.message || 'Unable to complete this action.');
    return result.data;
  }
}
