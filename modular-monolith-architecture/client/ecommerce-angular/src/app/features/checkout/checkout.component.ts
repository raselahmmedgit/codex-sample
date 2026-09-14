import { CurrencyPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CartDto } from '../../core/models/commerce.models';
import { PageHeaderComponent } from '../../shared/components/page-header.component';
import { Address } from '../customer/customer.models';
import { CheckoutService } from './checkout.service';

@Component({
  selector: 'app-checkout',
  imports: [CurrencyPipe, ReactiveFormsModule, RouterLink, PageHeaderComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-header title="Checkout" description="Confirm your delivery details and review your order." />
    @if (errorMessage()) { <div class="alert alert-warning" role="alert">{{ errorMessage() }}</div> }
    @if (successMessage()) { <div class="alert alert-success" role="alert">{{ successMessage() }}</div> }

    @if (cart()?.items?.length) {
      <div class="row g-4 align-items-start">
        <div class="col-lg-7">
          <section class="checkout-section rounded-4 bg-white border p-4 mb-4">
            <div class="section-heading"><span class="step-number">1</span><div><h2 class="h5 mb-1">Delivery address</h2><p class="small text-secondary mb-0">Where should we deliver your order?</p></div></div>
            @if (addresses().length) {
              <div class="address-options mt-4">
                @for (address of addresses(); track address.id) {
                  <label class="address-option" [class.selected]="selectedAddressId() === address.id"><input type="radio" name="address" [value]="address.id" [checked]="selectedAddressId() === address.id" (change)="selectedAddressId.set(address.id)" /><span><strong>{{ address.line1 }}</strong><small>{{ address.city }}, {{ address.country }}</small></span>@if (address.isDefault) { <em>Default</em> }</label>
                }
              </div>
            } @else {
              <p class="text-secondary mt-4 mb-3">You do not have a saved address yet.</p><a class="btn btn-outline-primary btn-sm" routerLink="/profile">Add an address</a>
            }
          </section>

          <section class="checkout-section rounded-4 bg-white border p-4">
            <div class="section-heading"><span class="step-number">2</span><div><h2 class="h5 mb-1">Coupon code</h2><p class="small text-secondary mb-0">Have a discount code?</p></div></div>
            <form class="coupon-form mt-4" [formGroup]="couponForm" (ngSubmit)="applyCoupon()"><input class="form-control" formControlName="code" placeholder="Enter coupon code" aria-label="Coupon code" /><button class="btn btn-outline-primary" type="submit" [disabled]="couponForm.invalid || couponLoading">{{ couponLoading ? 'Checking…' : 'Apply' }}</button></form>
            @if (couponMessage()) { <p class="small text-success mt-2 mb-0">{{ couponMessage() }}</p> }
          </section>
        </div>

        <aside class="col-lg-5">
          <section class="summary-card rounded-4 bg-white border p-4">
            <h2 class="h5 mb-4">Order summary</h2>
            @for (item of cart()!.items; track item.id) { <div class="d-flex justify-content-between small mb-3"><span>Product {{ item.productId.slice(0, 8) }} × {{ item.quantity }}</span><span>{{ item.total | currency }}</span></div> }
            <hr />
            <div class="d-flex justify-content-between text-secondary mb-2"><span>Subtotal</span><span>{{ subtotal() | currency }}</span></div>
            <div class="d-flex justify-content-between text-secondary mb-2"><span>Shipping</span><span>{{ shippingCost() | currency }}</span></div>
            @if (discount() > 0) { <div class="d-flex justify-content-between text-success mb-2"><span>Discount</span><span>−{{ discount() | currency }}</span></div> }
            <hr />
            <div class="d-flex justify-content-between fw-bold fs-5 mb-4"><span>Total</span><span>{{ total() | currency }}</span></div>
            <button class="btn btn-primary btn-lg w-100" type="button" [disabled]="!selectedAddressId() || placingOrder" (click)="placeOrder()">{{ placingOrder ? 'Placing order…' : 'Place order' }}</button>
            @if (!selectedAddressId()) { <p class="small text-danger text-center mt-2 mb-0">Select a delivery address to continue.</p> }
          </section>
        </aside>
      </div>
    } @else if (!errorMessage()) {
      <div class="empty-state rounded-4 bg-white border p-5 text-center"><div class="empty-icon mb-3">🛒</div><h2 class="h5">There is nothing to checkout</h2><p class="text-secondary mb-4">Add products to your cart first.</p><a class="btn btn-primary" routerLink="/products">Browse products</a></div>
    }
  `,
  styles: `
    .checkout-section { box-shadow: 0 .5rem 1.25rem rgba(23, 32, 51, .04); }
    .section-heading { display: flex; align-items: center; gap: .75rem; }
    .step-number { display: grid; width: 2rem; height: 2rem; place-items: center; border-radius: 50%; background: #e9efff; color: var(--bs-primary); font-weight: 700; }
    .address-options { display: grid; gap: .75rem; }
    .address-option { display: flex; align-items: center; gap: .75rem; padding: 1rem; border: 1px solid #dce2ed; border-radius: .75rem; cursor: pointer; }
    .address-option.selected { border-color: var(--bs-primary); background: #f5f7ff; }
    .address-option span { display: grid; gap: .25rem; flex: 1; } .address-option small { color: #6b7484; } .address-option em { color: var(--bs-primary); font-size: .75rem; font-style: normal; font-weight: 600; }
    .coupon-form { display: flex; gap: .5rem; }
    .summary-card { position: sticky; top: 6rem; }
    .empty-state { min-height: 300px; display: grid; place-content: center; } .empty-icon { font-size: 3rem; }
    @media (max-width: 575.98px) { .coupon-form { flex-direction: column; } .coupon-form button { width: 100%; } }
  `
})
export class CheckoutComponent {
  private readonly checkout = inject(CheckoutService);
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  readonly cart = signal<CartDto | null>(null);
  readonly addresses = signal<Address[]>([]);
  readonly selectedAddressId = signal<string | null>(null);
  readonly discount = signal(0);
  readonly couponMessage = signal('');
  readonly errorMessage = signal('');
  readonly successMessage = signal('');
  readonly couponForm = this.fb.nonNullable.group({ code: ['', Validators.required] });
  readonly subtotal = computed(() => this.cart()?.total ?? 0);
  readonly shippingCost = computed(() => this.subtotal() > 100 ? 0 : 10);
  readonly total = computed(() => Math.max(0, this.subtotal() + this.shippingCost() - this.discount()));
  couponLoading = false;
  placingOrder = false;

  constructor() {
    this.checkout.getCart().subscribe({ next: (cart) => this.cart.set(cart), error: (error: Error) => this.errorMessage.set(error.message || 'Cart is unavailable.') });
    this.checkout.getAddresses().subscribe({ next: (addresses) => { this.addresses.set(addresses); this.selectedAddressId.set(addresses.find((address) => address.isDefault)?.id ?? addresses[0]?.id ?? null); }, error: (error: Error) => this.errorMessage.set(error.message || 'Addresses are unavailable.') });
  }

  applyCoupon(): void {
    if (this.couponForm.invalid || !this.cart()) return;
    this.couponLoading = true; this.couponMessage.set('');
    this.checkout.validateCoupon(this.couponForm.controls.code.value, this.subtotal()).subscribe({ next: (coupon) => { this.discount.set(coupon.discountAmount); this.couponMessage.set(`${coupon.code} applied successfully.`); this.couponLoading = false; }, error: (error: Error) => { this.discount.set(0); this.couponMessage.set(error.message || 'Coupon is invalid.'); this.couponLoading = false; } });
  }

  placeOrder(): void {
    if (!this.selectedAddressId() || this.placingOrder) return;
    this.placingOrder = true; this.errorMessage.set('');
    const orderNumber = `WEB-${Date.now()}`;
    this.checkout.createOrder(orderNumber).subscribe({ next: () => this.router.navigateByUrl('/orders'), error: (error: Error) => { this.errorMessage.set(error.message || 'Unable to place order.'); this.placingOrder = false; } });
  }
}
