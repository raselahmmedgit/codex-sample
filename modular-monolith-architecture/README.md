# E-Commerce Platform

Production-oriented modular monolith for an e-commerce platform. The implementation is being delivered incrementally according to the master prompt.

## Phase 1 status

Phase 1 establishes the compileable solution foundation:

- Clean dependency direction: API and Infrastructure depend on Application; Application depends on Domain.
- ASP.NET Core Web API on `net9.0` (the installed SDK/runtime baseline; upgrade to `net10.0` when the .NET 10 SDK is available).
- EF Core provider abstraction for SQL Server and MySQL.
- Strongly typed `DatabaseSettings` configuration.
- Swagger/OpenAPI with JWT bearer security definition.
- Serilog console and rolling file logging.
- Application-level `IAppLogger<T>` abstraction.
- Correlation ID middleware using `X-Correlation-ID`.
- Centralized exception-handling middleware with standardized error JSON.
- `/health`, `/health/live`, and `/health/ready` endpoints.

## Prerequisites

- .NET SDK 9.x
- SQL Server or MySQL for database-backed features in later phases
- Visual Studio 2022+, VS Code, or .NET CLI

## Run

```powershell
dotnet restore --source https://api.nuget.org/v3/index.json
dotnet build ECommerce.sln
dotnet run --project src/ECommerce.API/ECommerce.API.csproj
```

Development Swagger is available at `/swagger`.

## Database provider

Set `Database:Provider` to `SqlServer` or `MySql` and provide `ConnectionStrings:DefaultConnection`. Provider-specific EF Core setup is isolated in `src/ECommerce.Infrastructure/DependencyInjection.cs`.

## Tests

```powershell
dotnet test ECommerce.sln
```

Later phases will add domain modules, Identity/JWT, catalog and order workflows, Angular UI, migrations, Docker, Nginx, CI/CD, and production deployment documentation.
