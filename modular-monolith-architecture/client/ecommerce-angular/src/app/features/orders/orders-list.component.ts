import { CurrencyPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PageHeaderComponent } from '../../shared/components/page-header.component';
import { Order } from './orders.models';
import { OrdersService } from './orders.service';
import { orderStatusName } from './orders.constants';

@Component({
  selector: 'app-orders-list',
  imports: [CurrencyPipe, RouterLink, PageHeaderComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-header title="My orders" description="Track your purchases and view order details." />
    @if (errorMessage()) { <div class="alert alert-warning" role="alert">{{ errorMessage() }}</div> }
    @if (orders().length) {
      <div class="orders-list rounded-4 bg-white border overflow-hidden">
        @for (order of orders(); track order.id) {
          <article class="order-row p-3 p-md-4 d-flex align-items-center gap-3">
            <div class="order-icon">#</div>
            <div class="flex-grow-1"><h2 class="h6 mb-1">{{ order.orderNumber }}</h2><p class="small text-secondary mb-0">{{ order.items.length }} item{{ order.items.length === 1 ? '' : 's' }}</p></div>
            <span class="status-pill" [class]="statusClass(order.status)">{{ statusName(order.status) }}</span>
            <strong class="order-total">{{ order.total | currency }}</strong>
            <a class="btn btn-outline-primary btn-sm" [routerLink]="['/orders', order.id]">View</a>
          </article>
        }
      </div>
    } @else if (!errorMessage()) {
      <div class="empty-state rounded-4 bg-white border p-5 text-center"><div class="empty-icon mb-3">▤</div><h2 class="h5">No orders yet</h2><p class="text-secondary mb-4">Your completed orders will appear here.</p><a class="btn btn-primary" routerLink="/products">Start shopping</a></div>
    }
  `,
  styles: `
    .order-row + .order-row { border-top: 1px solid #edf0f5; }
    .order-icon { display: grid; width: 2.75rem; height: 2.75rem; place-items: center; border-radius: .8rem; background: #e9efff; color: var(--bs-primary); font-weight: 700; }
    .status-pill { padding: .35rem .6rem; border-radius: 99px; background: #f1f3f7; color: #596273; font-size: .75rem; font-weight: 700; }
    .status-pill.success { background: #eaf8ef; color: #18794e; } .status-pill.warning { background: #fff5df; color: #9a6700; }
    .order-total { min-width: 5rem; text-align: right; }
    .empty-state { min-height: 280px; display: grid; place-content: center; } .empty-icon { color: var(--bs-primary); font-size: 3rem; }
    @media (max-width: 575.98px) { .order-row { flex-wrap: wrap; } .order-total { margin-left: auto; } }
  `
})
export class OrdersListComponent {
  private readonly service = inject(OrdersService);
  readonly orders = signal<Order[]>([]);
  readonly errorMessage = signal('');
  constructor() { this.service.list().subscribe({ next: (orders) => this.orders.set(orders), error: (error: Error) => this.errorMessage.set(error.message || 'Orders are temporarily unavailable.') }); }
  statusName(status: number | string): string { return orderStatusName(status); }
  statusClass(status: number | string): string { const name = this.statusName(status); return name === 'Delivered' ? 'status-pill success' : ['Pending', 'Confirmed'].includes(name) ? 'status-pill warning' : 'status-pill'; }
}
