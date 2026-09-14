import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PageHeaderComponent } from '../../shared/components/page-header.component';
import { AdminService } from './admin.service';

@Component({
  selector: 'app-admin',
  imports: [PageHeaderComponent, ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-header title="Admin workspace" description="Manage the commerce platform from one operational view." />
    @if (message()) { <div class="alert alert-success" role="alert">{{ message() }}</div> }
    @if (errorMessage()) { <div class="alert alert-warning" role="alert">{{ errorMessage() }}</div> }
    <div class="admin-layout">
      <aside class="admin-nav rounded-4 bg-white border p-2">
        @for (item of sections; track item.key) { <button type="button" [class.active]="activeSection() === item.key" (click)="activeSection.set(item.key)"><span>{{ item.icon }}</span>{{ item.label }}</button> }
      </aside>
      <section class="admin-content">
        @if (activeSection() === 'overview') { <div class="row g-3 mb-4">@for (card of cards; track card.label) { <div class="col-sm-6 col-xl-3"><div class="metric-card rounded-4 bg-white border p-4"><span class="metric-icon">{{ card.icon }}</span><strong>{{ card.value }}</strong><span>{{ card.label }}</span></div></div> }</div><div class="rounded-4 bg-white border p-4"><h2 class="h5">Quick actions</h2><p class="text-secondary">Use the workspace navigation to create catalog records, adjust inventory and issue coupons.</p><div class="d-flex flex-wrap gap-2"><button class="btn btn-primary btn-sm" type="button" (click)="activeSection.set('product')">Add product</button><button class="btn btn-outline-primary btn-sm" type="button" (click)="activeSection.set('inventory')">Adjust inventory</button><button class="btn btn-outline-primary btn-sm" type="button" (click)="activeSection.set('coupon')">Create coupon</button></div></div> }
        @if (activeSection() === 'product') { <form class="admin-card rounded-4 bg-white border p-4" [formGroup]="productForm" (ngSubmit)="createProduct()"><h2 class="h5 mb-4">Create product</h2><div class="row g-3"><div class="col-md-4"><label class="form-label">SKU</label><input class="form-control" formControlName="sku" /></div><div class="col-md-8"><label class="form-label">Name</label><input class="form-control" formControlName="name" /></div><div class="col-12"><label class="form-label">Description</label><textarea class="form-control" rows="3" formControlName="description"></textarea></div><div class="col-md-4"><label class="form-label">Price</label><input class="form-control" type="number" min="0" formControlName="price" /></div></div><button class="btn btn-primary mt-4" type="submit" [disabled]="productForm.invalid">Create product</button></form> }
        @if (activeSection() === 'category') { <form class="admin-card rounded-4 bg-white border p-4" [formGroup]="categoryForm" (ngSubmit)="createCategory()"><h2 class="h5 mb-4">Create category</h2><label class="form-label">Category name</label><input class="form-control" formControlName="name" /><button class="btn btn-primary mt-4" type="submit" [disabled]="categoryForm.invalid">Create category</button></form> }
        @if (activeSection() === 'brand') { <form class="admin-card rounded-4 bg-white border p-4" [formGroup]="brandForm" (ngSubmit)="createBrand()"><h2 class="h5 mb-4">Create brand</h2><label class="form-label">Brand name</label><input class="form-control" formControlName="name" /><button class="btn btn-primary mt-4" type="submit" [disabled]="brandForm.invalid">Create brand</button></form> }
        @if (activeSection() === 'inventory') { <form class="admin-card rounded-4 bg-white border p-4" [formGroup]="inventoryForm" (ngSubmit)="adjustInventory()"><h2 class="h5 mb-4">Adjust inventory</h2><div class="row g-3"><div class="col-md-7"><label class="form-label">Product ID</label><input class="form-control" formControlName="productId" placeholder="Product GUID" /></div><div class="col-md-5"><label class="form-label">Quantity</label><input class="form-control" type="number" min="0" formControlName="quantity" /></div><div class="col-12"><label class="form-label">Reason</label><input class="form-control" formControlName="reason" placeholder="Stock replenishment" /></div></div><button class="btn btn-primary mt-4" type="submit" [disabled]="inventoryForm.invalid">Save inventory</button></form> }
        @if (activeSection() === 'coupon') { <form class="admin-card rounded-4 bg-white border p-4" [formGroup]="couponForm" (ngSubmit)="createCoupon()"><h2 class="h5 mb-4">Create coupon</h2><div class="row g-3"><div class="col-md-5"><label class="form-label">Code</label><input class="form-control" formControlName="code" placeholder="WELCOME10" /></div><div class="col-md-4"><label class="form-label">Type</label><select class="form-select" formControlName="type"><option [value]="0">Percentage</option><option [value]="1">Fixed amount</option></select></div><div class="col-md-3"><label class="form-label">Value</label><input class="form-control" type="number" min="0" formControlName="value" /></div></div><button class="btn btn-primary mt-4" type="submit" [disabled]="couponForm.invalid">Create coupon</button></form> }
        @if (activeSection() === 'reviews') { <div class="admin-card rounded-4 bg-white border p-4"><h2 class="h5">Review moderation</h2><p class="text-secondary">Review moderation endpoints are not available in the current API contract.</p><span class="badge rounded-pill text-bg-light text-secondary">Pending backend API</span></div> }
      </section>
    </div>
  `,
  styles: `
    .admin-layout { display: grid; grid-template-columns: 220px 1fr; gap: 1.25rem; }
    .admin-nav { display: grid; align-content: start; gap: .25rem; height: max-content; position: sticky; top: 6rem; }
    .admin-nav button { display: flex; align-items: center; gap: .65rem; padding: .7rem .75rem; border: 0; border-radius: .6rem; background: transparent; color: #596273; text-align: left; font-size: .88rem; font-weight: 600; }
    .admin-nav button:hover, .admin-nav button.active { background: #f0f4ff; color: var(--bs-primary); }
    .metric-card { display: grid; gap: .35rem; } .metric-card strong { font-size: 1.65rem; } .metric-card span:last-child { color: #6b7484; font-size: .8rem; } .metric-icon { color: var(--bs-primary); font-size: 1.3rem; }
    .admin-card { max-width: 760px; } .form-label { font-size: .86rem; font-weight: 600; color: #354052; }
    @media (max-width: 767.98px) { .admin-layout { grid-template-columns: 1fr; } .admin-nav { position: static; display: flex; overflow-x: auto; } .admin-nav button { white-space: nowrap; } }
  `
})
export class AdminComponent {
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(AdminService);
  readonly activeSection = signal('overview');
  readonly message = signal(''); readonly errorMessage = signal('');
  readonly sections = [{ key: 'overview', label: 'Overview', icon: '▦' }, { key: 'product', label: 'Products', icon: '▣' }, { key: 'category', label: 'Categories', icon: '◈' }, { key: 'brand', label: 'Brands', icon: '◆' }, { key: 'inventory', label: 'Inventory', icon: '◫' }, { key: 'coupon', label: 'Coupons', icon: '%' }, { key: 'reviews', label: 'Reviews', icon: '★' }];
  readonly cards = [{ icon: '▣', value: '—', label: 'Products' }, { icon: '▤', value: '—', label: 'Orders today' }, { icon: '◫', value: '—', label: 'Low stock' }, { icon: '%', value: '—', label: 'Active coupons' }];
  readonly productForm = this.fb.nonNullable.group({ sku: ['', Validators.required], name: ['', Validators.required], description: '', price: [0, [Validators.required, Validators.min(0)]] });
  readonly categoryForm = this.fb.nonNullable.group({ name: ['', Validators.required] });
  readonly brandForm = this.fb.nonNullable.group({ name: ['', Validators.required] });
  readonly inventoryForm = this.fb.nonNullable.group({ productId: ['', Validators.required], quantity: [0, [Validators.required, Validators.min(0)]], reason: ['', Validators.required] });
  readonly couponForm = this.fb.nonNullable.group({ code: ['', Validators.required], type: [0], value: [0, [Validators.required, Validators.min(0.01)]] });
  private action<T>(request: import('rxjs').Observable<T>, success: string): void { this.message.set(''); this.errorMessage.set(''); request.subscribe({ next: () => this.message.set(success), error: (error: Error) => this.errorMessage.set(error.message || 'Admin action failed.') }); }
  createProduct(): void { if (this.productForm.valid) this.action(this.service.createProduct(this.productForm.getRawValue()), 'Product created successfully.'); }
  createCategory(): void { if (this.categoryForm.valid) this.action(this.service.createCategory({ ...this.categoryForm.getRawValue(), parentCategoryId: null }), 'Category created successfully.'); }
  createBrand(): void { if (this.brandForm.valid) this.action(this.service.createBrand(this.brandForm.getRawValue()), 'Brand created successfully.'); }
  adjustInventory(): void { if (this.inventoryForm.valid) this.action(this.service.adjustInventory(this.inventoryForm.getRawValue()), 'Inventory adjusted successfully.'); }
  createCoupon(): void { if (this.couponForm.valid) this.action(this.service.createCoupon({ ...this.couponForm.getRawValue(), startsAtUtc: new Date().toISOString(), expiresAtUtc: new Date(Date.now() + 30 * 86400000).toISOString() }), 'Coupon created successfully.'); }
}
