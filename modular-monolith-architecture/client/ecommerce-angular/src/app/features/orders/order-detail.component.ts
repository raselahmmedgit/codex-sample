import { CurrencyPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PageHeaderComponent } from '../../shared/components/page-header.component';
import { ORDER_STATUSES, orderStatusName } from './orders.constants';
import { Order } from './orders.models';
import { OrdersService } from './orders.service';

@Component({
  selector: 'app-order-detail',
  imports: [CurrencyPipe, RouterLink, PageHeaderComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    @if (order(); as current) {
      <app-page-header [title]="current.orderNumber" description="Order details and payment status." />
      <a class="back-link d-inline-block mb-4" routerLink="/orders">← Back to orders</a>
      <section class="status-card rounded-4 bg-white border p-4 mb-4"><h2 class="h6 mb-4">Order progress</h2><div class="status-track">@for (step of statuses; track step; let index = $index) { <div class="status-step" [class.complete]="index <= currentStatus"><span>{{ index <= currentStatus ? '✓' : index + 1 }}</span><small>{{ step }}</small></div> }</div></section>
      <div class="row g-4 align-items-start"><div class="col-lg-7"><section class="rounded-4 bg-white border p-4"><h2 class="h5 mb-4">Items</h2>@for (item of current.items; track item.productId) { <div class="item-row d-flex gap-3 align-items-center py-3"><div class="item-icon">{{ item.productName.charAt(0) }}</div><div class="flex-grow-1"><strong>{{ item.productName }}</strong><p class="small text-secondary mb-0">{{ item.quantity }} × {{ item.unitPrice | currency }}</p></div><span>{{ item.total | currency }}</span></div> }</section></div><aside class="col-lg-5"><section class="summary-card rounded-4 bg-white border p-4"><h2 class="h5 mb-4">Payment</h2><div class="d-flex justify-content-between text-secondary mb-2"><span>Order total</span><span>{{ current.total | currency }}</span></div><hr /><div class="d-flex justify-content-between fw-bold mb-4"><span>Total</span><span>{{ current.total | currency }}</span></div>@if (paymentMessage()) { <div class="alert alert-success small" role="alert">{{ paymentMessage() }}</div> }<button class="btn btn-primary w-100" type="button" [disabled]="paying || paymentMessage()" (click)="pay(current)">{{ paying ? 'Processing…' : 'Pay now' }}</button><p class="small text-secondary text-center mt-3 mb-0">Secure payment powered by the configured provider.</p></section></aside></div>
    } @else if (errorMessage()) { <div class="alert alert-warning" role="alert">{{ errorMessage() }}</div><a class="btn btn-outline-primary" routerLink="/orders">Back to orders</a> }
  `,
  styles: `
    .back-link { color: var(--bs-primary); font-weight: 600; text-decoration: none; }
    .status-track { display: flex; justify-content: space-between; gap: .5rem; position: relative; } .status-track::before { content: ''; position: absolute; top: 1rem; left: 1rem; right: 1rem; height: 2px; background: #e1e6ef; }
    .status-step { z-index: 1; display: grid; justify-items: center; gap: .45rem; color: #8a93a2; font-size: .75rem; text-align: center; } .status-step span { display: grid; width: 2rem; height: 2rem; place-items: center; border: 2px solid #e1e6ef; border-radius: 50%; background: #fff; } .status-step.complete { color: var(--bs-primary); font-weight: 600; } .status-step.complete span { border-color: var(--bs-primary); background: var(--bs-primary); color: #fff; }
    .item-row + .item-row { border-top: 1px solid #edf0f5; } .item-icon { display: grid; width: 2.75rem; height: 2.75rem; place-items: center; border-radius: .7rem; background: #e9efff; color: var(--bs-primary); font-weight: 700; }
    .summary-card { position: sticky; top: 6rem; }
    @media (max-width: 575.98px) { .status-step small { max-width: 3.5rem; font-size: .65rem; } }
  `
})
export class OrderDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly service = inject(OrdersService);
  readonly statuses = ORDER_STATUSES;
  readonly order = signal<Order | null>(null);
  readonly errorMessage = signal('');
  readonly paymentMessage = signal('');
  paying = false;
  constructor() { const id = this.route.snapshot.paramMap.get('id'); if (!id) { this.errorMessage.set('Order was not found.'); return; } this.service.get(id).subscribe({ next: (order) => this.order.set(order), error: (error: Error) => this.errorMessage.set(error.message || 'Order was not found.') }); }
  get currentStatus(): number { const current = this.order(); const name = current ? orderStatusName(current.status) : 'Pending'; return Math.max(0, this.statuses.indexOf(name)); }
  pay(order: Order): void { this.paying = true; this.service.initiatePayment(order.id, order.total).subscribe({ next: (response) => { this.paymentMessage.set(`${response.message} Reference: ${response.reference}`); this.paying = false; }, error: (error: Error) => { this.errorMessage.set(error.message || 'Payment could not be completed.'); this.paying = false; } }); }
}
