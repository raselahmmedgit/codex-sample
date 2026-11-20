# Codex Chat History Index

এই index-এ 2026-11-20 তারিখে local Codex history API-তে পাওয়া ২১টি active/non-archived Codex chat-এর title, project এবং summary রাখা হয়েছে। Archived Codex chat এবং ChatGPT chat পাওয়া যায়নি। এটি concise history index; raw turn-by-turn transcript নয়। Multi-laptop update হিসেবে local Codex history-এর সীমা, Cloud Work এবং context rehydration workflow-ও এখানে উল্লেখ করা হয়েছে।

## Current chat

| Title | Thread ID | Project | Summary |
|---|---|---|---|
| Codex sync-এর জন্য history spec লিখুন | `01a1f9de-993d-7a60-b804-5cfe40b992af` | `codex-sample` | `codex-sample` chat ও project history spec laptop-off backup folder-এ রাখা |

## Other Codex chats

| Title | Project/path | Summary |
|---|---|---|
| সব Excel শিট পর্যালোচনা করুন | `waltz-fashions-ltd-project` | old-ERP ও Google Sheet-এর সব Excel শিট বাংলায় বিস্তারিত review |
| All Microservice Details | `D:\GitLabCore` | microservice projects, source folder, architecture এবং feature development বোঝা |
| Understand the full project | `bracBankOneAppProject_Study` | OneApp project structure ও components exploration |
| Understand the full project | `bracBankOneAppCoreProject_Study` | OneApp Core structure ও main components review |
| Explain Postman step by step | `bracBankOneApp_Docs` | Postman ব্যবহারের step-by-step guide |
| Review Assisted Mode Journey | `bracBanlOneApp_Figma` | BRAC Bank OneApp Assisted Mode Journey Figma summary |
| Summarize Figma design details | `bracBanlOneApp_Figma` | DRD-Visual Figma node 481-7749 review |
| Review Brillio Bundle Journey design | `bracBankOneApp_Docs` | PL-CASA standalone bundle journey Figma review |
| Explain listed technology topics | `bracBankOneApp_Docs` | Grafana, OpenProject, WSO2 IAM, Git, Nexus, Jenkins, Figma, Vault, Kafka, Redis, Oracle, MongoDB overview |
| Cust. Journey Development | `bracBankOneApp_Docs` | BRAC customer journey development Figma workflow |
| Review BRAC Figma journey | `bracBanlOneApp_Figma` | customer journey Figma node 2025-83466 review |
| Process Flow | `bracBanlOneApp_Figma` | process steps, branches এবং decision points summary |
| Review DRD Visual Figma | `bracBanlOneApp_Figma` | DRD-Visual node 1211-48888 review |
| Export all sheets to text files | `C:\Users\rasel.ahmmed\Documents\Codex\2026-11-08\co` | প্রতিটি sheet একই নামে text/Notepad file export |
| Build, commit, and run | `outsourcing-baygroup-project` | BayGroup project build, commit এবং run |
| Bay_Group_CPS_SRS_Phase1_M01_M03_v1.2 | `outsourcing-baygroup-project` | main branch SRS M01/M02/M03 completion review |
| Review the full project | `outsourcing-baygroup-project` | BayGroup codebase, architecture এবং key components বোঝা |
| main branch | `outsourcing-baygroup-project` | seed password, SuperAdmin account, reset tooling এবং sign-in investigation |

## Known project outcomes and cautions

- BayGroup M01-M03 work completed with configurable UIC default, workflow, Angular wiring, migration fixes এবং targeted verification; production deployment claim করা হয়নি।
- BayGroup seeded password external secret থেকে আসে; plaintext password এই backup-এ রাখা যাবে না।
- OneApp/Figma summaries-এর কিছু field-level detail canvas scale বা unavailable board-এর কারণে inferred; exact BRD/SRS দিয়ে confirm করতে হবে।
- GitLabCore capture review-তে `ApplicationStartDate` UTC হিসেবে observed হয়েছিল; `AccountOpenDate`/`EmiDate` capture এবং EMI timing-এর সিদ্ধান্ত recheck করা দরকার।
- Postman, OneApp architecture এবং technology overview-এর বাংলা learning context retained in prior work summaries, কিন্তু raw transcript নয়।

## Rehydrating a chat on another laptop

নতুন chat শুরু করে এই তথ্য দিন:

```text
Use the synced backup at codex laptop file back-up/laptop-off.
Read PROJECT_INVENTORY.md, CHAT_HISTORY_INDEX.md and WORKSPACE_SNAPSHOT.md.
My target context is: <project title/path and chat title from the index>.
Continue from the recorded summary, then verify the current repository before changing anything.
```

প্রতিটি পুরোনো chat-এর full internal transcript এই folder-এ export করা হয়নি। তাই title/summary দিয়ে context restore করার পর current source, Git status এবং requirements পুনরায় verify করা আবশ্যক।

## Multi-laptop chat rules

- Local Codex chat-এর history ও local task context প্রতিটি laptop-এ সম্পূর্ণভাবে mirror হয় না। এই index হলো portable reference, internal database backup নয়।
- একই conversation বহু device-এ চালিয়ে যেতে হলে Cloud Work conversation ব্যবহার করুন, যদি আপনার account/workspace-এ তা available থাকে।
- Local repository-ভিত্তিক কাজের জন্য প্রতিটি laptop-এ repository open/register করে এই index থেকে target chat context rehydrate করুন।
- কোনো running local task অন্য laptop থেকে দেখতে বা control করতে হলে task যে host-এ চলছে সেটি online রাখুন এবং available Remote access ব্যবহার করুন।
- Cross-device cloud coding দরকার হলে Codex Cloud environment ব্যবহার করুন; local files-এর জন্য প্রত্যেক laptop-এর checkout, permission এবং dependency আলাদা করে যাচাই করুন।

## Standard context-restore prompt

```text
Read the synced Codex context files:
- codex laptop file back-up/laptop-off/read-me.txt
- codex laptop file back-up/laptop-off/PROJECT_INVENTORY.md
- codex laptop file back-up/laptop-off/CHAT_HISTORY_INDEX.md
- codex laptop file back-up/laptop-off/WORKSPACE_SNAPSHOT.md

Target project: <project name and local path>
Target previous chat: <chat title>

Continue from the recorded summary, then verify the current repository, Git status,
configuration and requirements before making any change.
```
