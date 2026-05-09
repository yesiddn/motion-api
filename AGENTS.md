# AGENTS.md — motion-api

## Stack
- .NET 10 minimal API (web, no controllers)
- EF Core 10 + Npgsql (PostgreSQL)
- Single-project solution (`motion-api.sln`)

## Setup & Run
```bash
docker compose up -d          # PostgreSQL + pgAdmin (localhost:5432 / :5050)
dotnet ef database update     # Apply migrations (DB: motion_db) — run after docker is up
dotnet run                    # http://localhost:5012  |  https://localhost:7093
```

- `appsettings.Development.json` holds the connection string for local dev.
- `.env` is loaded at startup by `DotNetEnv`. It must exist — contains `ADMIN_KEY` and any secrets.

## Architecture
- **Thin `Program.cs`**: only DI registration and endpoint mapping. Business logic lives in `Application/`, not in endpoints or `Program.cs`.
- Service pattern: interface (`IReportService`) + impl (`ReportService`), registered as scoped.
- Endpoints use `Results<T1,T2,...>` for OpenAPI-typed responses and a discriminated-union pattern via `CreateReportResult` (status enum + optional payload).

### Directory layout
| Directory | Purpose |
|-----------|---------|
| `Application/` | Services, DTOs/records, business logic |
| `Infrastructure/` | EF Core `IEntityTypeConfiguration<T>` classes |
| `Filters/` | Endpoint filters (auth) — namespace `motion_api.Presentation.Filters` |
| `Migrations/` | EF Core migrations |
| Root `.cs` files | Domain entities (`Node`, `Report`, `ApiKey`, `EventType`) |

## Database Quirks
- **Node PK is `MacAddress`** (string, max 17 chars) — not an auto-increment int.
- **`EventType` is stored as a string** in the DB via `HasConversion<string>()`. Acceptable values: `"Heartbeat"`, `"Motion"`.
- **ApiKeys are stored as SHA256 hashes** (Base64-encoded). Do NOT store or compare plaintext keys.
- EF configurations are auto-discovered via `ApplyConfigurationsFromAssembly` — just create a class implementing `IEntityTypeConfiguration<T>` in `Infrastructure/` and it's picked up.

## Auth
Two endpoint filter layers:
- **`ApiKeyFilter`** — `x-api-key` header, hashed lookup against `ApiKeys` table, attached per route group.
- **`AdminKeyFilter`** — `x-admin-key` header, compared against `ADMIN_KEY` env var.

## Testing
**No test project exists.** There is no test runner, no test framework installed, no CI workflow. If adding tests, start from scratch.

## Conventions
- Records for DTOs and request/response models (`CreateReportRequest`, `ReportResponse`, etc.).
- File-scoped namespaces.
- `Nullable` enabled project-wide.
- Prefer separating business logic into services under `Application/`; API body and responses must be JSON.
