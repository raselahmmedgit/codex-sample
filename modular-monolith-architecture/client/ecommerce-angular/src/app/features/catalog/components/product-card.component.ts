import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Product } from '../catalog.models';

@Component({
  selector: 'app-product-card',
  imports: [RouterLink, CurrencyPipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <article class="product-card h-100 rounded-4 bg-white border overflow-hidden">
      <div class="product-visual"><span>{{ product().name.charAt(0) }}</span></div>
      <div class="p-3 p-lg-4">
        <div class="d-flex justify-content-between align-items-start gap-2 mb-2">
          <h2 class="h6 mb-0">{{ product().name }}</h2>
          <span class="badge rounded-pill text-bg-light text-primary">{{ product().status }}</span>
        </div>
        <p class="small text-secondary mb-3">{{ product().description || 'Thoughtfully selected for everyday use.' }}</p>
        <div class="d-flex justify-content-between align-items-center">
          <strong class="price">{{ product().price | currency }}</strong>
          <a class="btn btn-sm btn-outline-primary" [routerLink]="['/products', product().id]">View details</a>
        </div>
      </div>
    </article>
  `,
  styles: `
    .product-card { transition: transform .2s ease, box-shadow .2s ease; }
    .product-card:hover { transform: translateY(-3px); box-shadow: 0 .75rem 1.5rem rgba(23, 32, 51, .08); }
    .product-visual { display: grid; height: 170px; place-items: center; background: linear-gradient(135deg, #e9efff, #f7f8fc); color: var(--bs-primary); font-size: 4rem; font-weight: 800; }
    .price { color: #172033; }
  `
})
export class ProductCardComponent {
  readonly product = input.required<Product>();
}
