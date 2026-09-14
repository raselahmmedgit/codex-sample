import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

function matchingPasswords(control: AbstractControl): ValidationErrors | null {
  const password = control.get('password')?.value;
  const confirmation = control.get('confirmPassword')?.value;
  return password && confirmation && password !== confirmation ? { passwordMismatch: true } : null;
}

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="auth-page">
      <div class="auth-card card rounded-4 p-4 p-md-5">
        <div class="auth-brand mb-4">E</div>
        <h1 class="h3 mb-2">Create your account</h1>
        <p class="text-secondary mb-4">Join Elevate Commerce and start shopping with confidence.</p>

        @if (errorMessage) { <div class="alert alert-danger" role="alert">{{ errorMessage }}</div> }

        <form [formGroup]="form" (ngSubmit)="submit()" novalidate>
          <div class="mb-3">
            <label class="form-label" for="displayName">Full name</label>
            <input id="displayName" class="form-control" type="text" formControlName="displayName" autocomplete="name" />
            @if (form.controls.displayName.touched && form.controls.displayName.invalid) { <div class="form-error mt-1">Name is required.</div> }
          </div>
          <div class="mb-3">
            <label class="form-label" for="email">Email address</label>
            <input id="email" class="form-control" type="email" formControlName="email" autocomplete="email" />
            @if (form.controls.email.touched && form.controls.email.invalid) { <div class="form-error mt-1">Enter a valid email address.</div> }
          </div>
          <div class="mb-3">
            <label class="form-label" for="password">Password</label>
            <input id="password" class="form-control" type="password" formControlName="password" autocomplete="new-password" />
            @if (form.controls.password.touched && form.controls.password.invalid) { <div class="form-error mt-1">Use at least 8 characters.</div> }
          </div>
          <div class="mb-4">
            <label class="form-label" for="confirmPassword">Confirm password</label>
            <input id="confirmPassword" class="form-control" type="password" formControlName="confirmPassword" autocomplete="new-password" />
            @if (form.hasError('passwordMismatch') && form.controls.confirmPassword.touched) { <div class="form-error mt-1">Passwords do not match.</div> }
          </div>
          <button class="btn btn-primary w-100 py-2" type="submit" [disabled]="form.invalid || submitting">
            {{ submitting ? 'Creating account…' : 'Create account' }}
          </button>
        </form>
        <p class="text-center text-secondary small mt-4 mb-0">Already have an account? <a routerLink="/login">Sign in</a></p>
      </div>
    </section>
  `,
  styleUrl: '../auth-form.scss'
})
export class RegisterComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  readonly form = this.fb.nonNullable.group({
    displayName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', Validators.required]
  }, { validators: matchingPasswords });
  submitting = false;
  errorMessage = '';

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.submitting = true;
    this.errorMessage = '';
    const { displayName, email, password } = this.form.getRawValue();
    this.auth.register({ displayName, email, password }).subscribe({
      next: () => this.router.navigateByUrl('/'),
      error: (error: Error) => { this.errorMessage = error.message || 'Unable to create your account.'; this.submitting = false; }
    });
  }
}
