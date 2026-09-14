import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { UiStateService } from '../services/ui-state.service';

export const loadingInterceptor: HttpInterceptorFn = (request, next) => {
  const uiState = inject(UiStateService);
  uiState.beginRequest();

  return next(request).pipe(finalize(() => uiState.endRequest()));
};
