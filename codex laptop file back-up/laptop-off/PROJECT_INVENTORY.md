# Codex Project Inventory

এই তালিকাটি 2026-11-20 তারিখে local Codex project registry থেকে তৈরি এবং multi-laptop use-এর জন্য update করা হয়েছে। `projectId`-গুলো Codex-এর internal identifier; অন্য laptop-এ এগুলো অপরিবর্তিত নাও থাকতে পারে। অন্য laptop-এ repository path এবং project registration পুনরায় যাচাই করতে হবে।

| Project label | Source path | Git repository | প্রধান context |
|---|---|---:|---|
| `codex-sample` | `C:\Users\rasel.ahmmed\Documents\GitHub\codex-sample` | Yes | Codex sample, modular monolith demo, laptop backup |
| `waltzFashionsLtd_Project` | `C:\Users\rasel.ahmmed\Documents\GitHub\waltz-fashions-ltd-project` | Yes | ERP/Excel sheet review |
| `bracBanlOneApp_GitLabCore` | `D:\GitLabCore` | Yes | BRAC Bank OneApp microservices |
| `bracBanlOneApp_Figma` | `C:\Users\rasel.ahmmed\Documents\GitHub\bbl-sample\bracBanlOneApp_Figma` | Yes | Figma journey and process-flow review |
| `bracBankOneApp_Docs` | `C:\Users\rasel.ahmmed\Documents\GitHub\bbl-sample\bracBankOneApp_Docs` | Yes | OneApp documentation and Postman learning |
| `bracBankOneAppProject_Study` | `C:\Users\rasel.ahmmed\Documents\GitHub\bbl-sample\bracBankOneAppProject_Study` | Yes | OneApp project study |
| `bracBankOneAppCoreProject_Study` | `C:\Users\rasel.ahmmed\Documents\GitHub\bbl-sample\bracBankOneAppCoreProject_Study` | Yes | OneApp Core architecture and build |
| `bay-cps` | `C:\Users\rasel.ahmmed\Documents\GitHub\outsourcing-baygroup-project` | Yes | BayGroup CPS procurement system |

## Repository snapshot: `codex-sample`

বর্তমান repository-তে প্রধানত আছে:

- `modular-monolith-architecture/`: ASP.NET Core 9 modular monolith E-Commerce sample
- `modular-monolith-architecture/src/`: API, Application, Domain এবং Infrastructure layers
- `modular-monolith-architecture/client/`: Angular 20 frontend
- `modular-monolith-architecture/database-scripts/`: SQL Server idempotent script
- `modular-monolith-architecture/docs/`: deployment, PostgreSQL এবং backup/restore notes
- `modular-monolith-architecture/postman/`: Postman collection এবং environment
- `modular-monolith-architecture/tests/`: unit ও integration test projects
- `kids-study/`, `personal-study/`: study notes placeholder folders
- `codex laptop file back-up/`: laptop sync backup artifacts

## Architecture context

এই sample-এ Clean Architecture ও modular-monolith approach ব্যবহার করা হয়েছে। API layer request/response পরিচালনা করে; Application layer use-case ও service orchestration করে; Domain layer entity/invariant রাখে; Infrastructure layer persistence, authentication, logging এবং external implementation দেয়। Angular client API-এর consumer এবং Docker/Nginx deployment support অন্তর্ভুক্ত।

## Portability notes

- Windows absolute path অন্য laptop-এ আলাদা হলে path remap করতে হবে।
- `.env`, JWT secret, database password এবং production credential backup-এ রাখা হয়নি।
- Build/test চালাতে `.NET SDK 9.x`, Node.js 22.x এবং প্রয়োজনীয় database/Docker runtime লাগতে পারে।
- অন্য laptop-এ প্রথমে repository clone/sync করে তারপর Codex project হিসেবে register করুন।

## Multi-laptop sync policy

এই inventory নিজে Codex desktop app-এর internal database sync করে না। Recommended ownership হলো:

| Data | Source of truth | অন্য laptop-এ করণীয় |
|---|---|---|
| Source code, migrations, tests | Git remote | `clone` বা `pull` করে current branch checkout করুন |
| এই project/chat specification | এই `laptop-off` folder, preferably Git-tracked | `pull` করে Markdown ও JSON পড়ুন |
| Cloud Work conversation | ChatGPT account-এর Cloud Work | একই account দিয়ে sign in করে conversation open করুন |
| Local Codex task | যে laptop/host-এ task চলছে | সেই host-এ কাজ চালান বা Remote access ব্যবহার করুন |
| Codex Cloud task | Codex Cloud environment | Cloud environment থেকে continue করুন |
| Secrets and credentials | Secret Manager বা secure local store | প্রতিটি laptop-এ আলাদাভাবে configure করুন |

## New laptop onboarding checklist

1. Required repository clone করুন এবং `git status` দিয়ে branch/worktree যাচাই করুন।
2. `codex laptop file back-up/laptop-off/` folder update করুন।
3. `CODEX_SYNC_MANIFEST.json`, `CHAT_HISTORY_INDEX.md` এবং `WORKSPACE_SNAPSHOT.md` পড়ুন।
4. Codex desktop app-এ local repository project হিসেবে register করুন।
5. Environment variables, SDK, Node.js, database এবং Docker dependency আলাদাভাবে configure করুন।
6. Secret file copy না করে secure secret store থেকে values দিন।
7. নতুন chat-এ target project এবং target chat title উল্লেখ করে context restore করুন।

## Conflict rule

দুই laptop-এ একই file একসঙ্গে edit করবেন না। কাজ শুরুর আগে `git pull`, কাজ শেষে focused commit ও `git push` করুন। Backup specification-এ conflict হলে latest verified source code এবং Git history-কে authority হিসেবে ধরে manual merge করুন।
