import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { UiStateService } from '../../core/services/ui-state.service';

@Component({
  selector: 'app-error-banner',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    @if (uiState.errorMessage(); as message) {
      <div class="container error-banner-wrap" role="alert">
        <div class="alert alert-warning d-flex align-items-center justify-content-between gap-3 mb-0">
          <span>{{ message }}</span>
          <button class="btn-close" type="button" aria-label="Dismiss error" (click)="uiState.clearError()"></button>
        </div>
      </div>
    }
  `,
  styles: `.error-banner-wrap { position: fixed; top: 4.75rem; left: 0; right: 0; z-index: 1050; }`
})
export class ErrorBannerComponent {
  readonly uiState = inject(UiStateService);
}
