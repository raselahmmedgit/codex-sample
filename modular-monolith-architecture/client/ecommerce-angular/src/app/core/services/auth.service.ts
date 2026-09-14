import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, finalize, map, tap } from 'rxjs';
import { ApiResult, AuthResponse, CurrentUser, LoginRequest, RegisterRequest } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly accessTokenKey = 'ecommerce.accessToken';
  private readonly refreshTokenKey = 'ecommerce.refreshToken';
  readonly isAuthenticated = signal(this.hasAccessToken());

  constructor(private readonly http: HttpClient) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<ApiResult<AuthResponse>>('/api/auth/login', request).pipe(map(result => this.unwrap(result)), tap(response => this.store(response)));
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<ApiResult<AuthResponse>>('/api/auth/register', request).pipe(map(result => this.unwrap(result)), tap(response => this.store(response)));
  }

  refresh(): Observable<AuthResponse> {
    return this.http.post<ApiResult<AuthResponse>>('/api/auth/refresh-token', { refreshToken: this.getRefreshToken() }).pipe(map(result => this.unwrap(result)), tap(response => this.store(response)));
  }

  logout(): Observable<unknown> {
    return this.http.post('/api/auth/logout', {}).pipe(finalize(() => this.clearSession()));
  }

  clearSession(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    this.isAuthenticated.set(false);
  }

  getAccessToken(): string | null { return localStorage.getItem(this.accessTokenKey); }
  getRefreshToken(): string | null { return localStorage.getItem(this.refreshTokenKey); }

  hasAnyRole(roles: string[]): boolean {
    const token = this.getAccessToken();
    if (!token) return false;
    try {
      const payload = JSON.parse(atob(token.split('.')[1])) as Record<string, string | string[]>;
      const claim = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? payload['role'];
      const tokenRoles = Array.isArray(claim) ? claim : claim ? [claim] : [];
      return roles.some((role) => tokenRoles.includes(role));
    } catch { return false; }
  }

  me(): Observable<CurrentUser> {
    return this.http.get<CurrentUser>('/api/auth/me');
  }

  private store(response: AuthResponse): void {
    localStorage.setItem(this.accessTokenKey, response.accessToken);
    localStorage.setItem(this.refreshTokenKey, response.refreshToken);
    this.isAuthenticated.set(true);
  }

  private hasAccessToken(): boolean { return !!localStorage.getItem(this.accessTokenKey); }

  private unwrap(result: ApiResult<AuthResponse>): AuthResponse {
    if (!result.succeeded || !result.data) throw new Error(result.errors?.join(', ') || result.message || 'Authentication failed.');
    return result.data;
  }
}
