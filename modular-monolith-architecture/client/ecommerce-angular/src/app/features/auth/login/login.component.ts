import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="auth-page">
      <div class="auth-card card rounded-4 p-4 p-md-5">
        <div class="auth-brand mb-4">E</div>
        <h1 class="h3 mb-2">Welcome back</h1>
        <p class="text-secondary mb-4">Sign in to continue to your account.</p>

        @if (errorMessage) { <div class="alert alert-danger" role="alert">{{ errorMessage }}</div> }

        <form [formGroup]="form" (ngSubmit)="submit()" novalidate>
          <div class="mb-3">
            <label class="form-label" for="email">Email address</label>
            <input id="email" class="form-control" type="email" formControlName="email" autocomplete="email" />
            @if (form.controls.email.touched && form.controls.email.invalid) { <div class="form-error mt-1">Enter a valid email address.</div> }
          </div>
          <div class="mb-4">
            <label class="form-label" for="password">Password</label>
            <input id="password" class="form-control" type="password" formControlName="password" autocomplete="current-password" maxlength="128" />
            @if (form.controls.password.touched && form.controls.password.invalid) { <div class="form-error mt-1">Enter a password with at least 8 characters.</div> }
          </div>
          <button class="btn btn-primary w-100 py-2" type="submit" [disabled]="form.invalid || submitting">
            {{ submitting ? 'Signing in…' : 'Sign in' }}
          </button>
        </form>
        <p class="text-center text-secondary small mt-4 mb-0">New here? <a routerLink="/register">Create an account</a></p>
      </div>
    </section>
  `,
  styleUrl: '../auth-form.scss'
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  readonly form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(128)]]
  });
  submitting = false;
  errorMessage = '';

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.submitting = true;
    this.errorMessage = '';
    this.auth.login(this.form.getRawValue()).subscribe({
      next: () => this.router.navigateByUrl('/'),
      error: (error: Error) => { this.errorMessage = error.message || 'Unable to sign in. Please try again.'; this.submitting = false; }
    });
  }
}
