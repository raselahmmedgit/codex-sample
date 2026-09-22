import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Product, productStatusLabel } from '../catalog.models';

@Component({
  selector: 'app-product-card',
  imports: [RouterLink, CurrencyPipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <article class="product-card h-100 rounded-4 bg-white border overflow-hidden">
      <div class="product-visual">
        <img [src]="product().imageUrl || '/images/product-catalog.png'" [alt]="product().name" />
      </div>
      <div class="p-3 p-lg-4">
        <div class="d-flex justify-content-between align-items-start gap-2 mb-2">
          <h2 class="h6 mb-0">{{ product().name }}</h2>
          <span class="badge rounded-pill text-bg-light text-primary">{{ statusLabel(product().status) }}</span>
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
    .product-visual { height: 170px; overflow: hidden; background: #f7f8fc; }
    .product-visual img { width: 100%; height: 100%; object-fit: cover; object-position: center; }
    .price { color: #172033; }
  `
})
export class ProductCardComponent {
  readonly product = input.required<Product>();
  statusLabel = productStatusLabel;
}
