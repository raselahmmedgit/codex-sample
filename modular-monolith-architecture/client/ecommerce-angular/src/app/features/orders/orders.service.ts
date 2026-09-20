import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of } from 'rxjs';
import { Order, PaymentResponse } from './orders.models';
import { demoOrders } from '../../core/data/demo-commerce.data';

interface ApiResult<T> { succeeded: boolean; data: T | null; message: string; errors: string[]; }

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private readonly http = inject(HttpClient);

  list(): Observable<Order[]> { return this.http.get<ApiResult<Order[]>>('/api/orders').pipe(map(result => this.unwrap(result)), catchError(() => of(demoOrders))); }
  get(id: string): Observable<Order> { return this.http.get<ApiResult<Order>>(`/api/orders/${id}`).pipe(map(result => this.unwrap(result)), catchError(() => of(demoOrders.find(order => order.id === id) ?? demoOrders[0]))); }
  initiatePayment(orderId: string, amount: number): Observable<PaymentResponse> { return this.http.post<ApiResult<PaymentResponse>>('/api/payments/initiate', { orderId, amount }).pipe(map(result => this.unwrap(result))); }

  private unwrap<T>(result: ApiResult<T>): T {
    if (!result.succeeded || result.data === null) throw new Error(result.message || 'Unable to load order information.');
    return result.data;
  }
}
