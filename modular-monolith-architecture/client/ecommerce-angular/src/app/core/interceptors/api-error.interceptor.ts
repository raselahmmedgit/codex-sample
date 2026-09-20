import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { UiStateService } from '../services/ui-state.service';

export const apiErrorInterceptor: HttpInterceptorFn = (request, next) => {
  const uiState = inject(UiStateService);
  return next(request).pipe(catchError((error: HttpErrorResponse) => {
    const message = typeof error.error?.message === 'string'
      ? error.error.message
      : error.status === 0 ? 'Unable to connect to the API.' : 'Something went wrong. Please try again.';
    const isCatalogRead = request.method === 'GET' && ['/api/products', '/api/categories', '/api/brands'].some(path => request.url.includes(path));
    if (!isCatalogRead) uiState.setError(message);
    return throwError(() => error);
  }));
};
