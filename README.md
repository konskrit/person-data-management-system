# Person Data Management System

Interview exercise: SQL Server schema + queries, then a three-layer .NET 10 app (EF Core, REST/JSON, Blazor + Fluent UI) for managing persons, addresses, and phone numbers.

## Stack

- .NET 10 / C#
- SQL Server Express
- EF Core (SqlServer)
- ASP.NET Core controllers + Scalar (OpenAPI UI in Development)
- Blazor Web App (Interactive Server) + Fluent UI Blazor 5
- DotNetEnv for `CONNECTION_STRING`

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server Express (`localhost\SQLEXPRESS`)
- Ability to run `.sql` scripts (SSMS, Azure Data Studio, or `sqlcmd`)

## Database setup

1. Run in order:

```text
sql/001_schema_and_seed.sql
sql/002_queries_and_maintenance.sql
```

`001` creates `PersonDataManagement`, tables (`Person`, `Address`, `Phone`) with `ON DELETE NO ACTION`, and seed data (including `Fischer` and intentional bad phone data).

`002` runs the assignment queries, creates `dbo.PersonDetails`, deletes phones not starting with `0` or `+`, and adds/fills `Person.LastNameUpper`.

2. Copy env config:

```bash
cp .env.example .env
```

Default connection string:

```text
Server=localhost\SQLEXPRESS;Database=PersonDataManagement;Trusted_Connection=True;TrustServerCertificate=True;
```

## Run

```bash
dotnet restore
dotnet run
```

- UI: [http://localhost:5048](http://localhost:5048)
- Scalar (Development): [http://localhost:5048/scalar](http://localhost:5048/scalar)

## What the app does

### REST API (`/api/persons`)

| Method   | Path                 | Description                                             |
| -------- | -------------------- | ------------------------------------------------------- |
| `GET`    | `/api/persons?name=` | List persons (optional first/last name contains filter) |
| `GET`    | `/api/persons/{id}`  | Detail with addresses and phones                        |
| `PUT`    | `/api/persons/{id}`  | Update first/last name                                  |
| `DELETE` | `/api/persons/{id}`  | Delete person (`409` if addresses/phones still exist)   |

### Blazor UI

- Empty Fluent DataGrid until **Load persons**
- Optional name filter
- Row click → detail dialog (addresses, phones)
- Edit name + **Save**
- **Delete** with confirmation; blocked when dependents remain

## Project layout

```text
sql/                  SQL schema, seed, assignment queries
Data/                 EF entities + PersonDbContext
Application/          DTOs, IPersonService, PersonService
Controllers/          PersonsController
Web/                  Blazor pages, layout, PersonDetailDialog
wwwroot/              App CSS
Program.cs            Host, DI, Scalar, Blazor
.env.example          Connection string template
```

## Architecture

Three layers share one host process:

1. **Data** — EF Core mapped to singular SQL tables; delete restrict on address/phone FKs
2. **Application** — service + record DTOs (no UI/HTTP concerns)
3. **Presentation** — REST controllers for JSON clients; Blazor UI calls the same application services

## Notes

- German characters use `NVARCHAR` + `N'…'` literals in SQL seed data
- Name search uses EF `Contains` (SQL `LIKE`), case-insensitive under typical SQL Server CI collations
- After changing `.env` or SQL scripts, restart the app / re-run scripts as needed
