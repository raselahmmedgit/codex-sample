import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResult, Brand, Category, PagedResult, Product } from './catalog.models';

@Injectable({ providedIn: 'root' })
export class CatalogService {
  private readonly http = inject(HttpClient);

  searchProducts(filters: { search?: string; categoryId?: string; brandId?: string; sortBy?: string; sortDescending?: boolean; pageNumber: number; pageSize: number }): Observable<PagedResult<Product>> {
    let params = new HttpParams().set('PageNumber', filters.pageNumber).set('PageSize', filters.pageSize);
    if (filters.search) params = params.set('Search', filters.search);
    if (filters.categoryId) params = params.set('CategoryId', filters.categoryId);
    if (filters.brandId) params = params.set('BrandId', filters.brandId);
    if (filters.sortBy) params = params.set('SortBy', filters.sortBy);
    params = params.set('SortDescending', filters.sortDescending ?? false);
    return this.http.get<ApiResult<PagedResult<Product>>>('/api/products', { params }).pipe(map(result => this.unwrap(result)));
  }

  getProduct(id: string): Observable<Product> {
    return this.http.get<ApiResult<Product>>(`/api/products/${id}`).pipe(map(result => this.unwrap(result)));
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<ApiResult<Category[]>>('/api/categories').pipe(map(result => this.unwrap(result)));
  }

  getBrands(): Observable<Brand[]> {
    return this.http.get<ApiResult<Brand[]>>('/api/brands').pipe(map(result => this.unwrap(result)));
  }

  private unwrap<T>(result: ApiResult<T>): T {
    if (!result.succeeded || result.data === null) throw new Error(result.message || 'Unable to load catalog data.');
    return result.data;
  }
}
