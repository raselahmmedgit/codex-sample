import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { CurrentUser } from '../../../core/models/auth.models';
import { Address } from '../customer.models';
import { CustomerService } from '../customer.service';
import { PageHeaderComponent } from '../../../shared/components/page-header.component';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule, PageHeaderComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-page-header title="Your profile" description="Manage your account details and delivery addresses." />

    @if (message()) { <div class="alert alert-success" role="alert">{{ message() }}</div> }
    @if (errorMessage()) { <div class="alert alert-warning" role="alert">{{ errorMessage() }}</div> }

    <section class="profile-card rounded-4 bg-white border p-4 p-md-5 mb-4">
      <div class="d-flex align-items-center gap-3">
        <div class="profile-avatar">{{ initials }}</div>
        <div>
          <h2 class="h5 mb-1">{{ user()?.displayName || 'Customer' }}</h2>
          <p class="text-secondary mb-0">{{ user()?.email || 'Account email' }}</p>
        </div>
      </div>
    </section>

    <div class="d-flex justify-content-between align-items-center mb-3">
      <div><h2 class="h4 mb-1">Delivery addresses</h2><p class="text-secondary small mb-0">Use a saved address during checkout.</p></div>
      <button class="btn btn-primary btn-sm" type="button" (click)="startCreate()">Add address</button>
    </div>

    @if (showForm()) {
      <form class="address-form rounded-4 bg-white border p-4 mb-4" [formGroup]="form" (ngSubmit)="saveAddress()" novalidate>
        <h3 class="h6 mb-3">{{ editingId() ? 'Edit address' : 'New address' }}</h3>
        <div class="row g-3">
          <div class="col-12"><label class="form-label" for="line1">Address line 1</label><input id="line1" class="form-control" formControlName="line1" />@if (form.controls.line1.touched && form.controls.line1.invalid) { <div class="field-error">Address line is required.</div> }</div>
          <div class="col-12"><label class="form-label" for="line2">Address line 2 <span class="text-secondary fw-normal">(optional)</span></label><input id="line2" class="form-control" formControlName="line2" /></div>
          <div class="col-md-6"><label class="form-label" for="city">City</label><input id="city" class="form-control" formControlName="city" />@if (form.controls.city.touched && form.controls.city.invalid) { <div class="field-error">City is required.</div> }</div>
          <div class="col-md-6"><label class="form-label" for="state">State / province</label><input id="state" class="form-control" formControlName="state" /></div>
          <div class="col-md-6"><label class="form-label" for="postalCode">Postal code</label><input id="postalCode" class="form-control" formControlName="postalCode" /></div>
          <div class="col-md-6"><label class="form-label" for="country">Country</label><input id="country" class="form-control" formControlName="country" />@if (form.controls.country.touched && form.controls.country.invalid) { <div class="field-error">Country is required.</div> }</div>
          <div class="col-12"><label class="form-check"><input class="form-check-input" type="checkbox" formControlName="isDefault" /><span class="form-check-label">Set as default address</span></label></div>
        </div>
        <div class="d-flex justify-content-end gap-2 mt-4"><button class="btn btn-light" type="button" (click)="cancelForm()">Cancel</button><button class="btn btn-primary" type="submit">Save address</button></div>
      </form>
    }

    @if (addresses().length) {
      <div class="row g-3">
        @for (address of addresses(); track address.id) {
          <article class="col-md-6"><div class="address-card rounded-4 bg-white border p-4 h-100"><div class="d-flex justify-content-between gap-2 mb-3"><span class="address-icon">⌂</span>@if (address.isDefault) { <span class="badge rounded-pill text-bg-light text-primary">Default</span> }</div><h3 class="h6">{{ address.line1 }}</h3>@if (address.line2) { <p class="mb-1">{{ address.line2 }}</p> }<p class="text-secondary mb-3">{{ address.city }}{{ address.state ? ', ' + address.state : '' }} {{ address.postalCode || '' }}<br />{{ address.country }}</p><div class="d-flex gap-2"><button class="btn btn-outline-primary btn-sm" type="button" (click)="startEdit(address)">Edit</button><button class="btn btn-link btn-sm text-danger" type="button" (click)="deleteAddress(address.id)">Delete</button></div></div></article>
        }
      </div>
    } @else if (!errorMessage() && !showForm()) {
      <div class="empty-state rounded-4 bg-white border p-5 text-center"><div class="empty-icon mb-3">⌂</div><h2 class="h5">No saved addresses</h2><p class="text-secondary mb-0">Add an address to make checkout faster.</p></div>
    }
  `,
  styles: `
    .profile-card { background: linear-gradient(120deg, #fff, #f5f7ff) !important; }
    .profile-avatar { display: grid; width: 3.5rem; height: 3.5rem; place-items: center; border-radius: 50%; background: #e9efff; color: var(--bs-primary); font-size: 1.25rem; font-weight: 700; }
    .form-label { color: #354052; font-size: .88rem; font-weight: 600; }
    .field-error { color: #b42318; font-size: .78rem; margin-top: .25rem; }
    .address-icon { color: var(--bs-primary); font-size: 1.5rem; }
    .empty-state { min-height: 230px; display: grid; place-content: center; }
    .empty-icon { color: var(--bs-primary); font-size: 3rem; }
  `
})
export class ProfileComponent {
  private readonly auth = inject(AuthService);
  private readonly customer = inject(CustomerService);
  private readonly fb = inject(FormBuilder);
  readonly user = signal<CurrentUser | null>(null);
  readonly addresses = signal<Address[]>([]);
  readonly showForm = signal(false);
  readonly editingId = signal<string | null>(null);
  readonly message = signal('');
  readonly errorMessage = signal('');
  readonly form = this.fb.nonNullable.group({ line1: ['', Validators.required], line2: '', city: ['', Validators.required], state: '', postalCode: '', country: ['', Validators.required], isDefault: false });

  constructor() {
    this.auth.me().subscribe({ next: (user) => this.user.set(user), error: () => undefined });
    this.loadAddresses();
  }

  get initials(): string { return (this.user()?.displayName || this.user()?.email || 'ME').split(' ').map((part) => part.charAt(0)).join('').slice(0, 2).toUpperCase(); }
  startCreate(): void { this.editingId.set(null); this.form.reset({ line1: '', line2: '', city: '', state: '', postalCode: '', country: '', isDefault: false }); this.showForm.set(true); }
  startEdit(address: Address): void {
    this.editingId.set(address.id);
    this.form.patchValue({ line1: address.line1, line2: address.line2 ?? '', city: address.city, state: address.state ?? '', postalCode: address.postalCode ?? '', country: address.country, isDefault: address.isDefault });
    this.showForm.set(true);
  }
  cancelForm(): void { this.showForm.set(false); }
  saveAddress(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const request = this.form.getRawValue();
    const action = this.editingId() ? this.customer.updateAddress(this.editingId()!, request) : this.customer.createAddress(request);
    action.subscribe({ next: () => { this.message.set('Address saved successfully.'); this.showForm.set(false); this.loadAddresses(); }, error: (error: Error) => this.errorMessage.set(error.message) });
  }
  deleteAddress(id: string): void { this.customer.deleteAddress(id).subscribe({ next: () => { this.message.set('Address deleted.'); this.loadAddresses(); }, error: (error: Error) => this.errorMessage.set(error.message) }); }
  private loadAddresses(): void { this.customer.listAddresses().subscribe({ next: (addresses) => this.addresses.set(addresses), error: (error: Error) => this.errorMessage.set(error.message || 'Addresses are temporarily unavailable.') }); }
}
