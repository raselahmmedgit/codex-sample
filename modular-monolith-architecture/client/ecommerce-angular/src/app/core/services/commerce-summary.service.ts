import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, of, forkJoin } from 'rxjs';
import { CartDto, WishlistDto } from '../models/commerce.models';

@Injectable({ providedIn: 'root' })
export class CommerceSummaryService {
  private readonly http = inject(HttpClient);
  readonly cartCount = signal(0);
  readonly wishlistCount = signal(0);

  refresh(): void {
    forkJoin({ cart: this.http.get<CartDto>('/api/cart').pipe(catchError(() => of(null))), wishlist: this.http.get<WishlistDto>('/api/wishlist').pipe(catchError(() => of(null))) }).subscribe(({ cart, wishlist }) => {
      this.cartCount.set(cart?.items.reduce((total, item) => total + item.quantity, 0) ?? 0);
      this.wishlistCount.set(wishlist?.items.length ?? 0);
    });
  }

  reset(): void {
    this.cartCount.set(0);
    this.wishlistCount.set(0);
  }
}
