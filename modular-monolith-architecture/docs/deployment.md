# Production Deployment Runbook

## Prerequisites

- Docker Engine and Docker Compose v2.
- A strong SQL Server password and a JWT signing key of at least 32 bytes.
- TLS termination at a managed load balancer or reverse proxy.

## Compose deployment

1. Copy `.env.example` to `.env` and replace every placeholder with a secret. Never commit `.env`.
2. Start the stack:

```powershell
docker compose up -d --build
```

3. Check service health:

```powershell
docker compose ps
docker compose logs --tail=100 api
```

4. Open the web application at `http://localhost:8088`. Nginx proxies `/api/*` to the API container.

Apply reviewed EF Core migrations in a controlled release step before production traffic. Do not enable development seed data in production.

## Security checklist

- Store `.env` values in a secret manager in real deployments.
- Terminate HTTPS before the web container and forward `X-Forwarded-Proto`.
- Keep SQL Server private; only the API should reach it.
- Disable development seed data and Swagger in production.
- Rotate JWT signing keys and refresh-token secrets according to the incident policy.
- Configure centralized logs, retention, alerts, and database backups.
