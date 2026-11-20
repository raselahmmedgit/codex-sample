# `codex-sample` Workspace Snapshot

Snapshot date: 2026-11-20, Asia/Dhaka

## Git state

- Repository: `C:\Users\rasel.ahmmed\Documents\GitHub\codex-sample`
- Current visible worktree: clean at snapshot time
- Recent commits:
  - `0eafe1e` — Merge branch `main` from GitHub
  - `e2cf1ee` — `2026-11-20_2`
  - `23a7899` — `2026-11-20`
  - `2693fca` — `2026-11-20`
  - `3cdd1de` — fix auth validation metadata for record parameters
  - `acb3c8e` — align identity forms with API validation
  - `8f650ad` — harden identity registration validation
  - `9c2cd34` — connect local development to PostgreSQL

## Main deliverable

`modular-monolith-architecture` একটি production-oriented E-Commerce sample। Backend-এ ASP.NET Core 9, EF Core, JWT/Identity, Serilog, correlation ID, global exception handling, catalog/cart/wishlist/customer/order/payment/inventory/review API আছে। Frontend-এ Angular 20 এবং Bootstrap styling আছে। Docker Compose, Nginx, SQL script, migrations, Swagger/OpenAPI, Postman এবং tests-ও আছে।

## Important verification points

- README-তে SQL Server/MySQL provider abstraction বলা হলেও repository-তে PostgreSQL configuration ও migration-ও রয়েছে; নতুন laptop-এ provider/configuration current source থেকে verify করুন।
- Development JWT placeholder, seed data এবং database access production-এ ব্যবহার করার আগে secret manager ও restricted configuration ব্যবহার করুন।
- `appsettings.*`, Postman environment এবং `.env`-জাতীয় file sync করার সময় secret value আছে কি না পরীক্ষা করুন।
- Backup/restore এবং deployment guidance `modular-monolith-architecture/docs/`-এ আছে।

## File-count style summary

- Backend solution: `ECommerce.sln`
- API project: `src/ECommerce.API`
- Application project: `src/ECommerce.Application`
- Domain project: `src/ECommerce.Domain`
- Infrastructure project: `src/ECommerce.Infrastructure`
- PostgreSQL migrations project: `src/ECommerce.Infrastructure.PostgreSql.Migrations`
- Test projects: `tests/ECommerce.UnitTests`, `tests/ECommerce.IntegrationTests`
- Angular app: `client/ecommerce-angular`
- Database scripts: `database-scripts`
- DevOps: `devops`, `docker-compose.yml`
- API client artifacts: `postman`

## Backup boundary

এই snapshot source code-এর পূর্ণ mirror নয়; এটি project/context specification। সম্পূর্ণ code sync করার জন্য Git remote, approved cloud storage বা repository clone ব্যবহার করুন।
