import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Review } from './review.models';

interface ApiResult<T> { succeeded: boolean; data: T | null; message: string; errors: string[]; }

@Injectable({ providedIn: 'root' })
export class ReviewService {
  private readonly http = inject(HttpClient);

  create(productId: string, rating: number, comment: string): Observable<Review> {
    return this.http.post<ApiResult<Review>>('/api/reviews', { productId, rating, comment }).pipe(map(result => this.unwrap(result)));
  }

  private unwrap<T>(result: ApiResult<T>): T {
    if (!result.succeeded || result.data === null) throw new Error(result.message || 'Unable to submit review.');
    return result.data;
  }
}
