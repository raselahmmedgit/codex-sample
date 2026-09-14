import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-main-layout',
  imports: [RouterLink, RouterLinkActive],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <header class="topbar border-bottom bg-white">
      <nav class="container d-flex align-items-center justify-content-between py-3" aria-label="Main navigation">
        <a class="brand text-decoration-none" routerLink="/">
          <span class="brand-mark">E</span>
          <span>Elevate Commerce</span>
        </a>
        <div class="d-flex align-items-center gap-2 gap-md-4">
          <a class="nav-link" routerLink="/products" routerLinkActive="active">Products</a>
          <a class="nav-link" routerLink="/cart" routerLinkActive="active">Cart</a>
          <a class="btn btn-primary btn-sm px-3" routerLink="/login">Sign in</a>
        </div>
      </nav>
    </header>

    <main class="container app-content py-4 py-md-5">
      <ng-content />
    </main>

    <footer class="border-top bg-white">
      <div class="container py-4 d-flex justify-content-between gap-3 flex-wrap text-secondary small">
        <span>© 2026 Elevate Commerce</span>
        <span>Secure modular commerce platform</span>
      </div>
    </footer>
  `,
  styles: `
    .topbar { position: sticky; top: 0; z-index: 1000; }
    .brand { display: inline-flex; align-items: center; gap: .65rem; color: #172033; font-weight: 700; }
    .brand-mark { display: grid; width: 2rem; height: 2rem; place-items: center; border-radius: .65rem; background: var(--bs-primary); color: white; }
    .nav-link { color: #5c6575; font-size: .95rem; font-weight: 600; }
    .nav-link:hover, .nav-link.active { color: var(--bs-primary); }
    .app-content { min-height: calc(100vh - 145px); }
  `
})
export class MainLayoutComponent {}
