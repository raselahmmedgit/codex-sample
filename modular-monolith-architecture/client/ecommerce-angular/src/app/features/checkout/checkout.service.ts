import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { CartDto } from '../../core/models/commerce.models';
import { Address } from '../customer/customer.models';

interface ApiResult<T> { succeeded: boolean; data: T | null; message: string; errors: string[]; }
export interface CouponDiscount { code: string; discountAmount: number; }
export interface CreatedOrder { id: string; orderNumber: string; status: string; total: number; }

@Injectable({ providedIn: 'root' })
export class CheckoutService {
  private readonly http = inject(HttpClient);

  getCart(): Observable<CartDto> { return this.http.get<ApiResult<CartDto>>('/api/cart').pipe(map(result => this.unwrap(result))); }
  getAddresses(): Observable<Address[]> { return this.http.get<ApiResult<Address[]>>('/api/customers/addresses').pipe(map(result => this.unwrap(result))); }
  validateCoupon(code: string, orderAmount: number): Observable<CouponDiscount> { return this.http.post<ApiResult<CouponDiscount>>('/api/coupons/validate', { code, orderAmount }).pipe(map(result => this.unwrap(result))); }
  createOrder(orderNumber: string): Observable<CreatedOrder> { return this.http.post<ApiResult<CreatedOrder>>('/api/orders', { orderNumber }).pipe(map(result => this.unwrap(result))); }

  private unwrap<T>(result: ApiResult<T>): T {
    if (!result.succeeded || result.data === null) throw new Error(result.message || 'Unable to complete checkout.');
    return result.data;
  }
}
