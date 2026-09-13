param(
    [string]$ConnectionString = $env:ECOMMERCE_CONNECTION_STRING
)

if ([string]::IsNullOrWhiteSpace($ConnectionString)) {
    throw "Provide ECOMMERCE_CONNECTION_STRING before applying migrations."
}

$env:ConnectionStrings__DefaultConnection = $ConnectionString
dotnet ef database update `
    --project src/ECommerce.Infrastructure/ECommerce.Infrastructure.csproj `
    --startup-project src/ECommerce.API/ECommerce.API.csproj `
    --context ApplicationDbContext
