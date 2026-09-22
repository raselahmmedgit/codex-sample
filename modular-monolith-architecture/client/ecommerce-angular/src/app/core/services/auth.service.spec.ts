import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({
      providers: [AuthService, provideHttpClient(), provideHttpClientTesting()]
    });
    service = TestBed.inject(AuthService);
    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpTesting.verify());

  it('shows Identity validation errors returned by the API', () => {
    let receivedError: Error | undefined;

    service.register({ displayName: 'Customer', email: 'customer@example.com', password: 'weak' }).subscribe({
      error: (error: Error) => receivedError = error
    });

    httpTesting.expectOne('/api/auth/register').flush(
      { succeeded: false, data: null, message: 'Registration failed.', errors: ['Passwords must include a digit.'] },
      { status: 400, statusText: 'Bad Request' }
    );

    expect(receivedError?.message).toBe('Passwords must include a digit.');
  });

  it('uses a safe fallback when the API error is not in the standard format', () => {
    let receivedError: Error | undefined;

    service.login({ email: 'customer@example.com', password: 'WrongPass1' }).subscribe({
      error: (error: Error) => receivedError = error
    });

    httpTesting.expectOne('/api/auth/login').flush('Unexpected response', {
      status: 500,
      statusText: 'Server Error'
    });

    expect(receivedError?.message).toBe('Authentication failed. Please try again.');
  });
});
