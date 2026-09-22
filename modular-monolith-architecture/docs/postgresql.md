# PostgreSQL support

The API supports SQL Server, MySQL and PostgreSQL through Entity Framework Core.

To run locally with PostgreSQL:

1. Start PostgreSQL and create the `ECommerceDb` database.
2. Set `Database:Provider` to `PostgreSql` and `Database:DefaultConnectionName` to `PostgreSqlConnection`.
3. Set the PostgreSQL password through .NET User Secrets; never commit it to configuration files.
4. Apply migrations with the PostgreSQL migrations project. Development configuration enables migrations and sample data seeding.
5. Run the API on port `5183`; the Angular development server proxies `/api` requests to it.

Initialize the local secret once, replacing the placeholder only in your terminal:

```powershell
dotnet user-secrets set "ConnectionStrings:PostgreSqlConnection" "Host=localhost;Port=5432;Database=ECommerceDb;Username=postgres;Password=YOUR_LOCAL_PASSWORD" --project src/ECommerce.API/ECommerce.API.csproj
```

Run the API with its Development launch profile (`http://localhost:5183`) and start the Angular app with `npm start` from `client/ecommerce-angular`. Development startup applies PostgreSQL migrations and seeds sample products.

Example environment variables in PowerShell:

```powershell
dotnet ef migrations add AddCatalogChange --context ApplicationDbContext --project src/ECommerce.Infrastructure.PostgreSql.Migrations --startup-project src/ECommerce.Infrastructure.PostgreSql.Migrations --output-dir Migrations
dotnet ef database update --context ApplicationDbContext --project src/ECommerce.Infrastructure.PostgreSql.Migrations --startup-project src/ECommerce.Infrastructure.PostgreSql.Migrations
```

The SQL Server migrations and PostgreSQL migrations are kept in separate assemblies because EF Core migrations can contain provider-specific SQL types and operations.
