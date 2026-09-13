# Database Operations

## SQL Server migration commands

Run from the repository root:

```powershell
$env:ECOMMERCE_DESIGN_CONNECTION = "Server=localhost;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True;"
dotnet ef migrations add InitialCreate --project src/ECommerce.Infrastructure --startup-project src/ECommerce.API --context ApplicationDbContext --output-dir Persistence/Migrations --source https://api.nuget.org/v3/index.json
dotnet ef database update --project src/ECommerce.Infrastructure --startup-project src/ECommerce.API --context ApplicationDbContext
dotnet ef migrations script --project src/ECommerce.Infrastructure --startup-project src/ECommerce.API --context ApplicationDbContext --idempotent -o database-scripts/sqlserver-idempotent.sql
```

Review generated migrations before production deployment. Do not run destructive migrations automatically in production.

## MySQL

Set `Database:Provider` to `MySql` and provide the MySQL connection string. The application provider selection is isolated inside Infrastructure. Generate and review a provider-specific migration or SQL script in a controlled environment before applying it to production.

```powershell
dotnet ef migrations add InitialCreateMySql --project src/ECommerce.Infrastructure --startup-project src/ECommerce.API --context ApplicationDbContext --output-dir Persistence/MigrationsMySql
dotnet ef database update --project src/ECommerce.Infrastructure --startup-project src/ECommerce.API --context ApplicationDbContext
```

## Startup policy

`Database:ApplyMigrationsOnStartup` and `Database:SeedDevelopmentData` are both `false` by default. Enable them only in a controlled Development environment. Production deployments should use a reviewed migration artifact after taking a database backup.
