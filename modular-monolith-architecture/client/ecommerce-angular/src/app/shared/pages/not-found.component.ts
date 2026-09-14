import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-not-found',
  imports: [RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="not-found text-center py-5" aria-labelledby="not-found-title">
      <div class="code">404</div>
      <h1 id="not-found-title" class="h3">Page not found</h1>
      <p class="text-secondary mb-4">The page you are looking for does not exist or has moved.</p>
      <a class="btn btn-primary" routerLink="/">Return home</a>
    </section>
  `,
  styles: `.not-found { min-height: 55vh; display: grid; place-content: center; } .code { color: var(--bs-primary); font-size: 6rem; font-weight: 800; line-height: 1; }`
})
export class NotFoundComponent {}
