# Meridian Vote Tracker

Home assignment: import Assembly vote exports, detect changes against approved data,
and let an analyst decide every conflict, with full traceability.

## Stack

- ASP.NET Core 9 Web API + Entity Framework Core
- PostgreSQL 16
- Angular (frontend)
- Docker Compose

## Run the project

From the repository root:

```bash
docker compose up --build
```

- Frontend: http://localhost:4200
- API: http://localhost:8080
- PostgreSQL: localhost:5433 (database `meridian`, user `meridian`)

At startup the API applies the EF Core migrations and loads the reference data:
the member states from `member_states.csv` and the users (Dana, Omar, Lena).

## Main API endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/users` | Users for the "Switch user" menu |
| POST | `/api/import` | Upload a CSV file (form-data: `file`, `userId`), processed in the background |
| GET | `/api/import` | Import history with live progress |
| GET | `/api/import/{id}/report` | Import report: counts per status and rows not stored |
| GET | `/api/conflicts` | Conflicts waiting for a decision |
| POST | `/api/conflicts/decision` | Accept or reject a conflict |

## How to try it

1. Open http://localhost:4200 and choose a user in the top bar
2. Upload `assembly_export_2024_q1.csv` → rows are imported; invalid, malformed and duplicate rows are reported
3. Upload `assembly_export_2024_q1_rev2.csv` → differences with the approved data become conflicts
4. Check the report: `GET /api/import/{id}/report`

## Tests

No automated tests were written in the time available (see DECISIONS.md).