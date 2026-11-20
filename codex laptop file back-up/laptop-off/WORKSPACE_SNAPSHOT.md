# `codex-sample` Workspace Snapshot

Snapshot date: 2026-11-20, Asia/Dhaka
Update purpose: multi-laptop Codex desktop workflow and portable project context

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

এই snapshot source code-এর পূর্ণ mirror নয়; এটি project/context specification। সম্পূর্ণ code sync করার জন্য Git remote, approved cloud storage বা repository clone ব্যবহার করুন। Codex desktop-এর local chat database, local settings, plugin credentials বা running task state এই snapshot-এর অংশ নয়।

## Multi-laptop operating model

### Laptop setup

প্রতিটি laptop-এ একই Git repository clone করে Codex desktop app-এ local project হিসেবে register করুন। Repository path laptopভেদে আলাদা হলে `PROJECT_INVENTORY.md`-এর source path-কে reference হিসেবে ব্যবহার করুন; path hard-code করে ধরে নেবেন না।

### Daily sync sequence

```powershell
git pull origin main
git status
# Work in Codex after confirming the correct branch and repository.
git add <focused-files>
git commit -m "Describe the completed change"
git push origin main
```

উপরের commands-এর `<focused-files>` placeholder বাস্তব file path দিয়ে replace করতে হবে। একই কাজ দুই laptop-এ parallel হলে আলাদা branch ব্যবহার করে পরে merge/review করুন।

### Context sync sequence

1. `laptop-off` folder-এর latest version pull করুন।
2. `CHAT_HISTORY_INDEX.md` থেকে target chat title নির্বাচন করুন।
3. `WORKSPACE_SNAPSHOT.md` এবং `PROJECT_INVENTORY.md` পড়ুন।
4. Codex-কে current Git status, source এবং configuration verify করতে বলুন।
5. Historical summary-কে guidance হিসেবে নিন; current repository-কে final authority হিসেবে যাচাই করুন।

### Cloud versus local

- Cloud Work conversation cross-device continuation-এর জন্য উপযোগী।
- Local Codex chat-এর history এবং task context host-specific হতে পারে।
- Codex Cloud coding task cloud environment-এ চালালে laptop পরিবর্তন করেও task continue করা যায়।
- Local task-এর ক্ষেত্রে task-running host online, accessible এবং প্রয়োজনীয় files/permissions সহ available থাকতে হবে।

## Security boundary

এই snapshot-এ secret values রাখা যাবে না। প্রতিটি laptop-এ JWT secret, database password, API key, certificate এবং external service credential secure secret store বা environment configuration দিয়ে সেট করতে হবে।
