import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

interface ApiResult<T> { succeeded: boolean; data: T | null; message: string; errors: string[]; }
export interface AdminProductRequest { sku: string; name: string; description: string; price: number; }
export interface AdminCategoryRequest { name: string; parentCategoryId: string | null; }
export interface AdminBrandRequest { name: string; }
export interface InventoryRequest { productId: string; quantity: number; reason: string; }
export interface CouponRequest { code: string; type: number; value: number; startsAtUtc: string; expiresAtUtc: string; }

@Injectable({ providedIn: 'root' })
export class AdminService {
  private readonly http = inject(HttpClient);
  createProduct(request: AdminProductRequest): Observable<unknown> { return this.http.post<ApiResult<unknown>>('/api/products', request).pipe(map(result => this.unwrap(result))); }
  createCategory(request: AdminCategoryRequest): Observable<unknown> { return this.http.post<ApiResult<unknown>>('/api/categories', request).pipe(map(result => this.unwrap(result))); }
  createBrand(request: AdminBrandRequest): Observable<unknown> { return this.http.post<ApiResult<unknown>>('/api/brands', request).pipe(map(result => this.unwrap(result))); }
  adjustInventory(request: InventoryRequest): Observable<unknown> { return this.http.post<ApiResult<unknown>>('/api/inventory/adjust', request).pipe(map(result => this.unwrap(result))); }
  createCoupon(request: CouponRequest): Observable<unknown> { return this.http.post<ApiResult<unknown>>('/api/coupons', request).pipe(map(result => this.unwrap(result))); }
  private unwrap<T>(result: ApiResult<T>): T { if (!result.succeeded) throw new Error(result.message || 'Admin action failed.'); return result.data as T; }
}
