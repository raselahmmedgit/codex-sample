import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
  selector: 'app-page-header',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <header class="page-header mb-4">
      <div>
        <p class="eyebrow mb-2">E-COMMERCE PLATFORM</p>
        <h1 class="h2 mb-1">{{ title() }}</h1>
        @if (description()) {
          <p class="text-secondary mb-0">{{ description() }}</p>
        }
      </div>
      <ng-content />
    </header>
  `,
  styles: `
    .page-header { display: flex; align-items: end; justify-content: space-between; gap: 1rem; }
    .eyebrow { color: var(--bs-primary); font-size: .72rem; font-weight: 700; letter-spacing: .12em; }
    @media (max-width: 576px) { .page-header { align-items: start; flex-direction: column; } }
  `
})
export class PageHeaderComponent {
  readonly title = input.required<string>();
  readonly description = input('');
}
