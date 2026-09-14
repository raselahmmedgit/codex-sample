import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { UiStateService } from '../../core/services/ui-state.service';

@Component({
  selector: 'app-loading-indicator',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    @if (uiState.isLoading() > 0) {
      <div class="global-loading" role="status" aria-label="Loading">
        <div class="progress-bar progress-bar-striped progress-bar-animated"></div>
      </div>
    }
  `,
  styles: `
    .global-loading { position: fixed; inset: 0 0 auto; z-index: 1100; height: 3px; }
    .progress-bar { width: 100%; height: 100%; background: var(--bs-primary); }
  `
})
export class LoadingIndicatorComponent {
  readonly uiState = inject(UiStateService);
}
