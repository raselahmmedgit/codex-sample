export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAtUtc: string;
}

export interface ApiResult<T> {
  succeeded: boolean;
  data: T | null;
  message: string;
  errors: string[];
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest extends LoginRequest {
  displayName: string;
}

export interface CurrentUser {
  id: string;
  email: string;
  displayName: string | null;
  roles: string[];
}
