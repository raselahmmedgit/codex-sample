import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { WishlistDto } from '../../../core/models/commerce.models';
import { PageHeaderComponent } from '../../../shared/components/page-header.component';
import { CommerceActionsService } from '../commerce-actions.service';

@Component({
  selector: 'app-wishlist',
  imports: [RouterLink, PageHeaderComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-header title="Your wishlist" description="Keep the products you want to revisit close at hand." />
    @if (errorMessage()) { <div class="alert alert-warning" role="alert">{{ errorMessage() }}</div> }
    @if (wishlist()?.items?.length) {
      <div class="row g-3 g-lg-4">
        @for (item of wishlist()!.items; track item.id) {
          <article class="col-12 col-sm-6 col-lg-4">
            <div class="wishlist-card rounded-4 bg-white border p-4 h-100">
              <div class="item-icon mb-3">♡</div>
              <h2 class="h6">Product {{ item.productId.slice(0, 8) }}</h2>
              <p class="small text-secondary">Saved for later</p>
              <div class="d-flex gap-2 mt-4">
                <a class="btn btn-outline-primary btn-sm" [routerLink]="['/products', item.productId]">View product</a>
                <button class="btn btn-link btn-sm text-danger" type="button" (click)="remove(item.id)">Remove</button>
              </div>
            </div>
          </article>
        }
      </div>
    } @else if (!errorMessage()) {
      <div class="empty-state rounded-4 bg-white border p-5 text-center">
        <div class="empty-icon mb-3">♡</div>
        <h2 class="h5">Your wishlist is empty</h2>
        <p class="text-secondary mb-4">Save products here when you find something special.</p>
        <a class="btn btn-primary" routerLink="/products">Explore products</a>
      </div>
    }
  `,
  styles: `
    .wishlist-card { transition: transform .2s ease, box-shadow .2s ease; }
    .wishlist-card:hover { transform: translateY(-3px); box-shadow: 0 .75rem 1.5rem rgba(23, 32, 51, .08); }
    .item-icon { display: grid; width: 3rem; height: 3rem; place-items: center; border-radius: .9rem; background: #fff0f1; color: #b42318; font-size: 1.5rem; }
    .empty-state { min-height: 300px; display: grid; place-content: center; }
    .empty-icon { color: #b42318; font-size: 3rem; }
  `
})
export class WishlistComponent {
  private readonly actions = inject(CommerceActionsService);
  readonly wishlist = signal<WishlistDto | null>(null);
  readonly errorMessage = signal('');

  constructor() { this.load(); }
  remove(id: string): void { this.actions.removeWishlistItem(id).subscribe({ next: () => this.load(), error: (error: Error) => this.errorMessage.set(error.message) }); }
  private load(): void { this.actions.getWishlist().subscribe({ next: (wishlist) => this.wishlist.set(wishlist), error: (error: Error) => this.errorMessage.set(error.message || 'Your wishlist is temporarily unavailable.') }); }
}
