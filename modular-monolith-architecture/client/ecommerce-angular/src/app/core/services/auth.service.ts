import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { AuthResponse, LoginRequest } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly accessTokenKey = 'ecommerce.accessToken';
  private readonly refreshTokenKey = 'ecommerce.refreshToken';
  readonly isAuthenticated = signal(this.hasAccessToken());

  constructor(private readonly http: HttpClient) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>('/api/auth/login', request).pipe(tap(response => this.store(response)));
  }

  refresh(): Observable<AuthResponse> {
    return this.http.post<AuthResponse>('/api/auth/refresh-token', { refreshToken: this.getRefreshToken() }).pipe(tap(response => this.store(response)));
  }

  logout(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    this.isAuthenticated.set(false);
  }

  getAccessToken(): string | null { return localStorage.getItem(this.accessTokenKey); }
  getRefreshToken(): string | null { return localStorage.getItem(this.refreshTokenKey); }

  private store(response: AuthResponse): void {
    localStorage.setItem(this.accessTokenKey, response.accessToken);
    localStorage.setItem(this.refreshTokenKey, response.refreshToken);
    this.isAuthenticated.set(true);
  }

  private hasAccessToken(): boolean { return !!localStorage.getItem(this.accessTokenKey); }
}
