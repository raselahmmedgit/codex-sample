import { ChangeDetectionStrategy, Component, inject, input, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { ReviewService } from './review.service';

@Component({
  selector: 'app-review-form',
  imports: [ReactiveFormsModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="review-panel rounded-4 bg-white border p-4">
      <div class="d-flex justify-content-between align-items-start gap-3 mb-3"><div><h2 class="h5 mb-1">Share your experience</h2><p class="small text-secondary mb-0">Reviews are limited to verified purchasers.</p></div><span class="verified-badge">✓ Verified only</span></div>
      @if (!auth.isAuthenticated()) {
        <div class="sign-in-prompt rounded-3 p-3"><p class="small mb-2">Sign in to leave a verified review after your order is delivered.</p><a class="btn btn-outline-primary btn-sm" routerLink="/login">Sign in</a></div>
      } @else if (submitted()) {
        <div class="alert alert-success mb-0" role="alert">Your review was submitted for moderation. Thank you!</div>
      } @else {
        @if (errorMessage()) { <div class="alert alert-warning" role="alert">{{ errorMessage() }}</div> }
        <form [formGroup]="form" (ngSubmit)="submit()" novalidate>
          <fieldset class="mb-3"><legend class="form-label mb-2">Your rating</legend><div class="star-picker" role="radiogroup" aria-label="Rating">@for (star of stars; track star) { <button type="button" [attr.aria-label]="star + ' stars'" [class.selected]="star <= form.controls.rating.value" (click)="setRating(star)">★</button> }</div>@if (form.controls.rating.touched && form.controls.rating.invalid) { <div class="field-error">Please select a rating.</div> }</fieldset>
          <div class="mb-3"><label class="form-label" for="comment">Your review</label><textarea id="comment" class="form-control" rows="4" formControlName="comment" placeholder="What did you think?"></textarea>@if (form.controls.comment.touched && form.controls.comment.invalid) { <div class="field-error">Please write at least 10 characters.</div> }</div>
          <button class="btn btn-primary" type="submit" [disabled]="form.invalid || submitting">{{ submitting ? 'Submitting…' : 'Submit review' }}</button>
        </form>
      }
    </section>
  `,
  styles: `
    .review-panel { box-shadow: 0 .5rem 1.25rem rgba(23, 32, 51, .04); }
    .verified-badge { padding: .35rem .55rem; border-radius: 99px; background: #eaf8ef; color: #18794e; font-size: .72rem; font-weight: 700; white-space: nowrap; }
    .sign-in-prompt { background: #f5f7ff; }
    .star-picker { display: flex; gap: .25rem; }
    .star-picker button { border: 0; padding: 0 .15rem; background: transparent; color: #d4dbe8; font-size: 1.8rem; line-height: 1; }
    .star-picker button.selected { color: #f5b940; }
    .form-label { color: #354052; font-size: .88rem; font-weight: 600; }
    .field-error { color: #b42318; font-size: .78rem; margin-top: .25rem; }
  `
})
export class ReviewFormComponent {
  readonly productId = input.required<string>();
  readonly stars = [1, 2, 3, 4, 5];
  readonly submitted = signal(false);
  readonly errorMessage = signal('');
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(ReviewService);
  readonly auth = inject(AuthService);
  readonly form = this.fb.nonNullable.group({ rating: [0, [Validators.min(1), Validators.max(5)]], comment: ['', [Validators.required, Validators.minLength(10)]] });
  submitting = false;

  setRating(rating: number): void { this.form.controls.rating.setValue(rating); this.form.controls.rating.markAsTouched(); }
  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.submitting = true; this.errorMessage.set('');
    const { rating, comment } = this.form.getRawValue();
    this.service.create(this.productId(), rating, comment).subscribe({ next: () => { this.submitted.set(true); this.submitting = false; }, error: (error: Error) => { this.errorMessage.set(error.message || 'Unable to submit review.'); this.submitting = false; } });
  }
}
