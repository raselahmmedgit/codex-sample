export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAtUtc: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}
