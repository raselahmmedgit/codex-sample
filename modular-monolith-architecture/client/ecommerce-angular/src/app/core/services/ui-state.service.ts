import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UiStateService {
  private readonly pendingRequests = signal(0);
  readonly isLoading = this.pendingRequests.asReadonly();
  readonly errorMessage = signal<string | null>(null);

  beginRequest(): void {
    this.pendingRequests.update((count) => count + 1);
  }

  endRequest(): void {
    this.pendingRequests.update((count) => Math.max(0, count - 1));
  }

  setError(message: string | null): void {
    this.errorMessage.set(message);
  }

  clearError(): void {
    this.errorMessage.set(null);
  }
}
