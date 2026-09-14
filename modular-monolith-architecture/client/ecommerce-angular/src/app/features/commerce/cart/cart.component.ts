import { CurrencyPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CartDto } from '../../../core/models/commerce.models';
import { PageHeaderComponent } from '../../../shared/components/page-header.component';
import { CommerceActionsService } from '../commerce-actions.service';

@Component({
  selector: 'app-cart',
  imports: [CurrencyPipe, RouterLink, PageHeaderComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-header title="Your cart" description="Review your items before continuing to checkout.">
      @if (cart()?.items?.length) { <button class="btn btn-outline-secondary btn-sm" type="button" (click)="clear()">Clear cart</button> }
    </app-page-header>

    @if (errorMessage()) { <div class="alert alert-warning" role="alert">{{ errorMessage() }}</div> }
    @if (cart()?.items?.length) {
      <div class="row g-4 align-items-start">
        <div class="col-lg-8">
          <div class="cart-list rounded-4 bg-white border overflow-hidden">
            @for (item of cart()!.items; track item.id) {
              <article class="cart-item p-3 p-md-4 d-flex gap-3 align-items-center">
                <div class="item-icon">{{ item.productId.charAt(0) }}</div>
                <div class="flex-grow-1">
                  <h2 class="h6 mb-1">Product {{ item.productId.slice(0, 8) }}</h2>
                  <p class="small text-secondary mb-2">Unit price: {{ item.unitPrice | currency }}</p>
                  <div class="quantity-control" aria-label="Quantity controls">
                    <button type="button" aria-label="Decrease quantity" [disabled]="item.quantity <= 1" (click)="update(item.id, item.quantity - 1)">−</button>
                    <span>{{ item.quantity }}</span>
                    <button type="button" aria-label="Increase quantity" (click)="update(item.id, item.quantity + 1)">+</button>
                  </div>
                </div>
                <div class="text-end">
                  <strong>{{ item.total | currency }}</strong>
                  <button class="remove-button d-block ms-auto mt-2" type="button" (click)="remove(item.id)">Remove</button>
                </div>
              </article>
            }
          </div>
        </div>
        <aside class="col-lg-4">
          <div class="summary-card rounded-4 bg-white border p-4">
            <h2 class="h5 mb-4">Order summary</h2>
            <div class="d-flex justify-content-between text-secondary mb-2"><span>Subtotal</span><span>{{ cart()!.total | currency }}</span></div>
            <div class="d-flex justify-content-between text-secondary mb-3"><span>Shipping</span><span>Calculated at checkout</span></div>
            <hr />
            <div class="d-flex justify-content-between fw-bold mb-4"><span>Total</span><span>{{ cart()!.total | currency }}</span></div>
            <a class="btn btn-primary w-100" routerLink="/checkout">Continue to checkout</a>
          </div>
        </aside>
      </div>
    } @else if (!errorMessage()) {
      <div class="empty-state rounded-4 bg-white border p-5 text-center">
        <div class="empty-icon mb-3">🛒</div>
        <h2 class="h5">Your cart is empty</h2>
        <p class="text-secondary mb-4">Explore the catalog and find something you love.</p>
        <a class="btn btn-primary" routerLink="/products">Browse products</a>
      </div>
    }
  `,
  styles: `
    .cart-item + .cart-item { border-top: 1px solid #edf0f5; }
    .item-icon { display: grid; width: 3.5rem; height: 3.5rem; flex: 0 0 3.5rem; place-items: center; border-radius: 1rem; background: #e9efff; color: var(--bs-primary); font-size: 1.5rem; font-weight: 800; }
    .quantity-control { display: inline-flex; align-items: center; gap: .8rem; border: 1px solid #dce2ed; border-radius: .5rem; padding: .15rem .4rem; }
    .quantity-control button { width: 1.5rem; border: 0; background: transparent; color: var(--bs-primary); font-size: 1.1rem; }
    .remove-button { border: 0; background: transparent; color: #b42318; font-size: .78rem; }
    .summary-card { position: sticky; top: 6rem; }
    .empty-state { min-height: 300px; display: grid; place-content: center; }
    .empty-icon { font-size: 3rem; }
  `
})
export class CartComponent {
  private readonly actions = inject(CommerceActionsService);
  readonly cart = signal<CartDto | null>(null);
  readonly errorMessage = signal('');

  constructor() { this.load(); }
  update(id: string, quantity: number): void { this.actions.updateCartItem(id, quantity).subscribe({ next: (cart) => this.cart.set(cart), error: (error: Error) => this.errorMessage.set(error.message) }); }
  remove(id: string): void { this.actions.removeCartItem(id).subscribe({ next: () => this.load(), error: (error: Error) => this.errorMessage.set(error.message) }); }
  clear(): void { this.actions.clearCart().subscribe({ next: () => this.load(), error: (error: Error) => this.errorMessage.set(error.message) }); }
  private load(): void { this.actions.getCart().subscribe({ next: (cart) => this.cart.set(cart), error: (error: Error) => this.errorMessage.set(error.message || 'Your cart is temporarily unavailable.') }); }
}
