import { ChangeDetectionStrategy, Component, effect, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { CommerceSummaryService } from '../core/services/commerce-summary.service';

@Component({
  selector: 'app-main-layout',
  imports: [RouterLink, RouterLinkActive],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <header class="topbar border-bottom bg-white">
      <nav class="container navbar-shell" aria-label="Main navigation">
        <a class="brand text-decoration-none" routerLink="/" (click)="closeMenus()">
          <span class="brand-mark">E</span>
          <span class="brand-name">Elevate Commerce</span>
        </a>

        <button class="menu-toggle" type="button" aria-label="Toggle navigation" [attr.aria-expanded]="mobileMenuOpen()" (click)="toggleMobileMenu()">
          <span></span><span></span><span></span>
        </button>

        <div class="nav-content" [class.open]="mobileMenuOpen()">
          <div class="nav-links">
            <a class="nav-link" routerLink="/products" routerLinkActive="active" (click)="closeMenus()">Products</a>
            @if (auth.isAuthenticated()) {
              <a class="nav-link nav-with-badge" routerLink="/wishlist" routerLinkActive="active" (click)="closeMenus()">
                Wishlist @if (summary.wishlistCount() > 0) { <span class="count-badge">{{ summary.wishlistCount() }}</span> }
              </a>
              <a class="nav-link nav-with-badge" routerLink="/cart" routerLinkActive="active" (click)="closeMenus()">
                Cart @if (summary.cartCount() > 0) { <span class="count-badge">{{ summary.cartCount() }}</span> }
              </a>
              <a class="nav-link" routerLink="/orders" routerLinkActive="active" (click)="closeMenus()">Orders</a>
            } @else {
              <a class="nav-link" routerLink="/cart" routerLinkActive="active" (click)="closeMenus()">Cart</a>
            }
          </div>

          <div class="account-area">
            @if (auth.isAuthenticated()) {
              <button class="account-trigger" type="button" [attr.aria-expanded]="accountMenuOpen()" (click)="toggleAccountMenu()">
                <span class="avatar">{{ initials }}</span>
                <span class="account-label">My account</span>
                <span class="chevron">⌄</span>
              </button>
              @if (accountMenuOpen()) {
                <div class="account-menu shadow-sm" role="menu">
                  <a routerLink="/profile" (click)="closeMenus()" role="menuitem">Profile</a>
                  <a routerLink="/orders" (click)="closeMenus()" role="menuitem">My orders</a>
                  <button type="button" (click)="signOut()" role="menuitem">Sign out</button>
                </div>
              }
            } @else {
              <a class="btn btn-primary btn-sm px-3" routerLink="/login" (click)="closeMenus()">Sign in</a>
            }
          </div>
        </div>
      </nav>
    </header>

    <main id="main-content" class="container app-content py-4 py-md-5" tabindex="-1">
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
    .navbar-shell { min-height: 4.5rem; display: flex; align-items: center; justify-content: space-between; gap: 1.25rem; }
    .brand { display: inline-flex; align-items: center; gap: .65rem; color: #172033; font-weight: 700; }
    .brand-mark { display: grid; width: 2rem; height: 2rem; place-items: center; border-radius: .65rem; background: var(--bs-primary); color: white; }
    .nav-content, .nav-links, .account-area { display: flex; align-items: center; }
    .nav-content { flex: 1; justify-content: space-between; gap: 1.5rem; }
    .nav-links { gap: .35rem; margin-left: 2rem; }
    .nav-link { color: #5c6575; font-size: .92rem; font-weight: 600; padding: .5rem .7rem; }
    .nav-link:hover, .nav-link.active { color: var(--bs-primary); }
    .nav-with-badge { display: inline-flex; align-items: center; gap: .35rem; }
    .count-badge { display: inline-grid; min-width: 1.2rem; height: 1.2rem; place-items: center; padding: 0 .25rem; border-radius: 99px; background: rgba(var(--bs-primary-rgb), .1); color: var(--bs-primary); font-size: .7rem; }
    .account-area { position: relative; }
    .account-trigger { display: inline-flex; align-items: center; gap: .5rem; border: 0; background: transparent; color: #354052; font-size: .9rem; font-weight: 600; }
    .avatar { display: grid; width: 2rem; height: 2rem; place-items: center; border-radius: 50%; background: #e9efff; color: var(--bs-primary); font-size: .75rem; font-weight: 700; }
    .chevron { color: #7b8494; font-size: 1.1rem; }
    .account-menu { position: absolute; top: calc(100% + .75rem); right: 0; min-width: 10rem; padding: .4rem; border: 1px solid #e5e9f1; border-radius: .75rem; background: #fff; }
    .account-menu a, .account-menu button { display: block; width: 100%; padding: .6rem .7rem; border: 0; border-radius: .5rem; background: transparent; color: #354052; text-align: left; text-decoration: none; font-size: .88rem; }
    .account-menu a:hover, .account-menu button:hover { background: #f3f6fb; color: var(--bs-primary); }
    .menu-toggle { display: none; border: 0; background: transparent; padding: .45rem; }
    .menu-toggle span { display: block; width: 1.35rem; height: 2px; margin: .25rem; background: #354052; }
    .app-content { min-height: calc(100vh - 145px); }
    @media (max-width: 767.98px) {
      .brand-name { font-size: .9rem; }
      .menu-toggle { display: block; }
      .nav-content { display: none; position: absolute; top: 4.5rem; left: 0; right: 0; flex-direction: column; align-items: stretch; gap: 0; padding: .75rem 1rem 1rem; border-bottom: 1px solid #e5e9f1; background: #fff; box-shadow: 0 .75rem 1.25rem rgba(23, 32, 51, .06); }
      .nav-content.open { display: flex; }
      .nav-links { align-items: stretch; flex-direction: column; gap: .15rem; margin-left: 0; }
      .nav-link { padding: .7rem .5rem; }
      .account-area { justify-content: stretch; padding-top: .6rem; border-top: 1px solid #eef1f6; }
      .account-trigger { padding: .55rem .5rem; }
      .account-menu { position: static; width: 100%; margin-top: .35rem; box-shadow: none !important; }
    }
  `
})
export class MainLayoutComponent {
  readonly auth = inject(AuthService);
  readonly summary = inject(CommerceSummaryService);
  readonly mobileMenuOpen = signal(false);
  readonly accountMenuOpen = signal(false);
  readonly initials = 'ME';

  constructor() {
    effect(() => {
      if (this.auth.isAuthenticated()) this.summary.refresh();
      else this.summary.reset();
    });
  }

  toggleMobileMenu(): void { this.mobileMenuOpen.update((open) => !open); }
  toggleAccountMenu(): void { this.accountMenuOpen.update((open) => !open); }
  closeMenus(): void { this.mobileMenuOpen.set(false); this.accountMenuOpen.set(false); }

  signOut(): void {
    this.closeMenus();
    this.auth.logout().subscribe({ error: () => this.auth.clearSession() });
  }
}
