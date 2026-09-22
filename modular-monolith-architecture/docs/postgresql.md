# PostgreSQL support

The API supports SQL Server, MySQL and PostgreSQL through Entity Framework Core.

To run locally with PostgreSQL:

1. Start PostgreSQL and create the `ECommerceDb` database.
2. Set `Database:Provider` to `PostgreSql` and `Database:DefaultConnectionName` to `PostgreSqlConnection`.
3. Set the PostgreSQL password through user secrets or an environment-specific configuration file.
4. Generate a provider-specific migration before enabling automatic migrations in a production environment.

Example environment variables in PowerShell:

```powershell
$env:ECOMMERCE_DESIGN_PROVIDER = "PostgreSql"
$env:ECOMMERCE_DESIGN_CONNECTION = "Host=localhost;Port=5432;Database=ECommerceDb;Username=postgres;Password=CHANGE_ME"
dotnet ef migrations add InitialPostgreSql --context ApplicationDbContext --project src/ECommerce.Infrastructure --startup-project src/ECommerce.API --output-dir Persistence/MigrationsPostgreSql
```

The existing SQL Server migrations are intentionally kept separate from PostgreSQL migrations because EF Core migrations can contain provider-specific SQL types and operations.
