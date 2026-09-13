# E-Commerce Platform

Production-oriented modular monolith built with ASP.NET Core 9, EF Core, SQL Server/MySQL provider abstraction, Angular 20, JWT authentication and Docker.

## Implemented phases

- Clean Architecture solution foundation and dependency boundaries.
- Domain entities and invariants for catalog, customers, cart, wishlist, coupons, orders, payments, inventory and reviews.
- Application services and repository abstractions.
- EF Core persistence, SQL Server migrations, idempotent script and development seed.
- ASP.NET Identity, JWT access tokens, hashed refresh-token rotation and lockout.
- Serilog structured logging, correlation IDs, slow-request telemetry and global errors.
- Catalog, cart, wishlist, customer address, coupon, checkout, mock payment, inventory and verified-review APIs.
- Angular frontend foundation with auth service, interceptor, route guard and Bootstrap styling.
- Swagger/OpenAPI, Postman collection/environment, unit/integration tests, Docker Compose, Nginx and CI validation.

## Prerequisites

- .NET SDK 9.x
- Node.js 22.x and npm
- SQL Server or MySQL for local non-container execution
- Docker Desktop for the container stack

## Run the backend locally

```powershell
dotnet restore ECommerce.sln --source https://api.nuget.org/v3/index.json
dotnet build ECommerce.sln
dotnet run --project src/ECommerce.API/ECommerce.API.csproj
```

Development Swagger is available at `/swagger`. Configure `Database:Provider` and `ConnectionStrings:DefaultConnection` in application settings or environment variables.

## Run the Angular frontend locally

```powershell
Set-Location client/ecommerce-angular
npm ci
npm start
```

## Run with Docker

```powershell
Copy-Item .env.example .env
# Edit .env and replace both values with strong secrets.
docker compose up -d --build
```

The web application is available at `http://localhost:8088`; Nginx proxies `/api/*` to the API container. See [docs/deployment.md](docs/deployment.md) for production controls.

## Tests and validation

```powershell
dotnet build ECommerce.sln --source https://api.nuget.org/v3/index.json -m:1
dotnet test ECommerce.sln --no-build --no-restore
Set-Location client/ecommerce-angular
npm ci
npm run build
```

The Postman collection is in `postman/ecommerce.postman_collection.json`. Database backup and restore guidance is in [docs/backup-restore.md](docs/backup-restore.md).

## Security notes

- Replace the development JWT placeholder with a secret-manager value in every deployed environment.
- Do not commit `.env`, credentials or production connection strings.
- Keep Swagger, seed data and database access restricted in production.
- Use HTTPS termination, centralized logs, key rotation and tested backups.
