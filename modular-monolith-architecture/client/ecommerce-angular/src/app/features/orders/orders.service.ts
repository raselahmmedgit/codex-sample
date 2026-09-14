import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Order, PaymentResponse } from './orders.models';

interface ApiResult<T> { succeeded: boolean; data: T | null; message: string; errors: string[]; }

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private readonly http = inject(HttpClient);

  list(): Observable<Order[]> { return this.http.get<ApiResult<Order[]>>('/api/orders').pipe(map(result => this.unwrap(result))); }
  get(id: string): Observable<Order> { return this.http.get<ApiResult<Order>>(`/api/orders/${id}`).pipe(map(result => this.unwrap(result))); }
  initiatePayment(orderId: string, amount: number): Observable<PaymentResponse> { return this.http.post<ApiResult<PaymentResponse>>('/api/payments/initiate', { orderId, amount }).pipe(map(result => this.unwrap(result))); }

  private unwrap<T>(result: ApiResult<T>): T {
    if (!result.succeeded || result.data === null) throw new Error(result.message || 'Unable to load order information.');
    return result.data;
  }
}
