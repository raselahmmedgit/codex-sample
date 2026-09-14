import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { catchError, forkJoin, of } from 'rxjs';
import { CatalogService } from '../catalog.service';
import { Brand, Category, PagedResult, Product } from '../catalog.models';
import { ProductCardComponent } from '../components/product-card.component';
import { PageHeaderComponent } from '../../../shared/components/page-header.component';

@Component({
  selector: 'app-product-list',
  imports: [ReactiveFormsModule, ProductCardComponent, PageHeaderComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-header title="Explore products" description="Find practical essentials selected for quality, design and everyday value." />

    <section class="filter-panel rounded-4 bg-white border p-3 p-md-4 mb-4" aria-label="Product filters">
      <form [formGroup]="filterForm" (ngSubmit)="search()" class="row g-3 align-items-end">
        <div class="col-12 col-lg-5">
          <label class="form-label small fw-semibold" for="search">Search products</label>
          <input id="search" class="form-control" formControlName="search" placeholder="Search by product name or SKU" />
        </div>
        <div class="col-6 col-lg-2">
          <label class="form-label small fw-semibold" for="category">Category</label>
          <select id="category" class="form-select" formControlName="categoryId">
            <option value="">All categories</option>
            @for (category of categories(); track category.id) { <option [value]="category.id">{{ category.name }}</option> }
          </select>
        </div>
        <div class="col-6 col-lg-2">
          <label class="form-label small fw-semibold" for="brand">Brand</label>
          <select id="brand" class="form-select" formControlName="brandId">
            <option value="">All brands</option>
            @for (brand of brands(); track brand.id) { <option [value]="brand.id">{{ brand.name }}</option> }
          </select>
        </div>
        <div class="col-8 col-lg-2">
          <label class="form-label small fw-semibold" for="sort">Sort by</label>
          <select id="sort" class="form-select" formControlName="sortBy" (change)="search()">
            <option value="Name">Name</option>
            <option value="Price">Price</option>
            <option value="CreatedAt">Newest</option>
          </select>
        </div>
        <div class="col-4 col-lg-1">
          <button class="btn btn-primary w-100" type="submit" aria-label="Search">Go</button>
        </div>
      </form>
    </section>

    @if (errorMessage()) {
      <div class="alert alert-warning rounded-4" role="alert">{{ errorMessage() }}</div>
    } @else if (products().length === 0) {
      <div class="empty-state rounded-4 bg-white border p-5 text-center">
        <div class="empty-icon mb-3">⌕</div>
        <h2 class="h5">No products found</h2>
        <p class="text-secondary mb-0">Try changing your search or filters.</p>
      </div>
    } @else {
      <div class="d-flex justify-content-between align-items-center mb-3">
        <p class="text-secondary small mb-0">{{ totalCount() }} products</p>
        <span class="small text-secondary">Page {{ pageNumber() }} of {{ totalPages() }}</span>
      </div>
      <div class="row g-3 g-lg-4">
        @for (product of products(); track product.id) {
          <div class="col-12 col-sm-6 col-xl-4"><app-product-card [product]="product" /></div>
        }
      </div>
      <nav class="d-flex justify-content-center gap-2 mt-4" aria-label="Product pages">
        <button class="btn btn-outline-secondary btn-sm" type="button" [disabled]="pageNumber() <= 1" (click)="changePage(pageNumber() - 1)">Previous</button>
        <button class="btn btn-outline-secondary btn-sm" type="button" [disabled]="pageNumber() >= totalPages()" (click)="changePage(pageNumber() + 1)">Next</button>
      </nav>
    }
  `,
  styles: `
    .filter-panel { box-shadow: 0 .5rem 1.25rem rgba(23, 32, 51, .04); }
    .form-label { color: #354052; }
    .empty-state { min-height: 280px; display: grid; place-content: center; }
    .empty-icon { color: var(--bs-primary); font-size: 3rem; }
  `
})
export class ProductListComponent {
  private readonly fb = inject(FormBuilder);
  private readonly catalog = inject(CatalogService);
  readonly filterForm = this.fb.nonNullable.group({ search: '', categoryId: '', brandId: '', sortBy: 'Name' });
  readonly products = signal<Product[]>([]);
  readonly categories = signal<Category[]>([]);
  readonly brands = signal<Brand[]>([]);
  readonly totalCount = signal(0);
  readonly totalPages = signal(0);
  readonly pageNumber = signal(1);
  readonly errorMessage = signal('');
  private readonly pageSize = 12;

  constructor() {
    this.loadFilters();
    this.search();
  }

  search(): void {
    this.pageNumber.set(1);
    this.loadProducts();
  }

  changePage(page: number): void {
    this.pageNumber.set(page);
    this.loadProducts();
  }

  private loadFilters(): void {
    forkJoin({ categories: this.catalog.getCategories().pipe(catchError(() => of([]))), brands: this.catalog.getBrands().pipe(catchError(() => of([]))) }).subscribe(({ categories, brands }) => {
      this.categories.set(categories);
      this.brands.set(brands);
    });
  }

  private loadProducts(): void {
    this.errorMessage.set('');
    this.catalog.searchProducts({ ...this.filterForm.getRawValue(), pageNumber: this.pageNumber(), pageSize: this.pageSize }).subscribe({
      next: (result: PagedResult<Product>) => { this.products.set(result.items); this.totalCount.set(result.totalCount); this.totalPages.set(result.totalPages); },
      error: (error: Error) => { this.products.set([]); this.errorMessage.set(error.message || 'Catalog is temporarily unavailable.'); }
    });
  }
}
