import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Address, AddressRequest } from './customer.models';

interface ApiResult<T> { succeeded: boolean; data: T | null; message: string; errors: string[]; }

@Injectable({ providedIn: 'root' })
export class CustomerService {
  private readonly http = inject(HttpClient);

  listAddresses(): Observable<Address[]> { return this.http.get<ApiResult<Address[]>>('/api/customers/addresses').pipe(map(result => this.unwrap(result))); }
  createAddress(request: AddressRequest): Observable<Address> { return this.http.post<ApiResult<Address>>('/api/customers/addresses', request).pipe(map(result => this.unwrap(result))); }
  updateAddress(id: string, request: AddressRequest): Observable<Address> { return this.http.put<ApiResult<Address>>(`/api/customers/addresses/${id}`, request).pipe(map(result => this.unwrap(result))); }
  deleteAddress(id: string): Observable<boolean> { return this.http.delete<ApiResult<boolean>>(`/api/customers/addresses/${id}`).pipe(map(result => this.unwrap(result))); }

  private unwrap<T>(result: ApiResult<T>): T {
    if (!result.succeeded || result.data === null) throw new Error(result.message || 'Unable to complete address action.');
    return result.data;
  }
}
