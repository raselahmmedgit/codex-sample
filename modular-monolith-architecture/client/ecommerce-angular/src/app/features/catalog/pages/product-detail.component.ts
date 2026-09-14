import { CurrencyPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CatalogService } from '../catalog.service';
import { Product } from '../catalog.models';
import { ReviewFormComponent } from '../../reviews/review-form.component';

@Component({
  selector: 'app-product-detail',
  imports: [CurrencyPipe, RouterLink, ReviewFormComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    @if (product(); as item) {
      <a class="back-link d-inline-block mb-4" routerLink="/products">← Back to products</a>
      <section class="row g-4 g-lg-5 align-items-start">
        <div class="col-lg-6"><div class="detail-visual rounded-4"><span>{{ item.name.charAt(0) }}</span></div></div>
        <div class="col-lg-6">
          <span class="badge rounded-pill text-bg-light text-primary mb-3">{{ item.status }}</span>
          <h1 class="display-6 fw-bold">{{ item.name }}</h1>
          <p class="text-secondary mb-4">{{ item.description || 'Thoughtfully selected for everyday use.' }}</p>
          <div class="price mb-4">{{ item.price | currency }}</div>
          <button class="btn btn-primary btn-lg px-4" type="button" disabled>Sign in to add to cart</button>
          <p class="small text-secondary mt-3">Cart actions will be enabled in the next catalog phase.</p>
        </div>
      </section>
      <div class="mt-5"><app-review-form [productId]="item.id" /></div>
    } @else if (errorMessage()) {
      <div class="alert alert-warning" role="alert">{{ errorMessage() }}</div>
      <a routerLink="/products" class="btn btn-outline-primary">Back to products</a>
    }
  `,
  styles: `
    .back-link { color: var(--bs-primary); font-weight: 600; text-decoration: none; }
    .detail-visual { display: grid; min-height: 420px; place-items: center; background: linear-gradient(135deg, #e9efff, #f7f8fc); color: var(--bs-primary); font-size: 10rem; font-weight: 800; }
    .price { color: #172033; font-size: 2rem; font-weight: 700; }
    @media (max-width: 767.98px) { .detail-visual { min-height: 260px; font-size: 7rem; } }
  `
})
export class ProductDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly catalog = inject(CatalogService);
  readonly product = signal<Product | null>(null);
  readonly errorMessage = signal('');

  constructor() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) { this.errorMessage.set('Product was not found.'); return; }
    this.catalog.getProduct(id).subscribe({ next: (product) => this.product.set(product), error: (error: Error) => this.errorMessage.set(error.message || 'Product was not found.') });
  }
}
