# Decisions

## 1. What I prioritised

With limited time, I focused on the core of the problem: **data integrity and traceability**.
Approved data is never overwritten silently, every row of every file is kept, and every change can be explained.

Done:
- CSV import in the background with live progress
- Row validation and import report (rows not stored, with original values and reason)
- Conflict detection (country vote or vote title) and human decision (accept / reject)
- History log of uploads, created votes, conflicts and decisions
- Angular UI: imports page and conflicts page
- Everything starts with `docker compose up`

Not done (time): snapshots / ranking, positions and weights UI, history screen, PDF bonus, automated tests.

## 2. Database design

- **MemberState / MembershipPeriod**: a state can leave and rejoin (e.g. Iceland), so membership is a list of periods.
- **Vote / CountryVote**: the CSV repeats the vote data on every country line; it is stored once in `Vote`.
  `CountryVote` holds the **current approved value**, one per (vote, country), enforced by a unique index.
- **Import**: one row per uploaded file (who, when, status, progress). The original file is kept on disk.
- **ImportRow**: one row per line of every file, with the original values (as strings, never fixed) and the outcome:
  New, Unchanged, Duplicate, Invalid, Conflict. This is the import report.
- **Conflict**: the approved value and the incoming value side by side, waiting for a decision.
- **HistoryEvent**: append-only log (never updated or deleted): who, what, when, old value, new value, source file.
- Enums are stored as text, so the database is readable and safe if an enum changes.
- Schema managed with EF Core migrations, applied automatically at startup.

## 3. Import rules

For each line, in this order:
1. **Invalid**: malformed line, unknown country code, invalid date, unknown ballot value
2. **Duplicate**: same vote and country already seen in this file
3. **New**: no approved value yet → stored
4. **Unchanged**: same as the approved value
5. **Conflict**: different from the approved value → never overwritten, a conflict is created

A vote missing from a new file is not treated as a deletion.

The import runs in the background (`Task.Run` with its own scope). Only one import runs at a time (`SemaphoreSlim`).
Progress is saved every 500 rows. If an import fails, it is marked Failed and the error is logged.

## 4. Conflicts

A conflict can be decided only once: the decision is applied only if the conflict is still Pending,
so if two analysts click at the same time, the second one gets a 409.
Accepting replaces the approved value (and its source file); rejecting keeps it. Both are written to the history.

## 5. Snapshots (not implemented, planned design)

Recalculating a published ranking from current data would change it after corrections, so results must be stored.
Plan: a `Snapshot` table (name, selection, created by, status, published quarter with a unique index)
and a `SnapshotResult` table (score, rank and per-vote breakdown per country), computed in the background.
Planned scoring rules: score = 100 × matching weight / counted weight; Abstain and Absent are not counted;
votes where the state was not a member are excluded; votes without a position block the snapshot.

## 6. Azure

I did not use Azure resources: the core requirements run locally with PostgreSQL in Docker,
and the PDF bonus (Document Intelligence) was not reached.
In production I would use Azure Database for PostgreSQL, Blob Storage for uploaded files,
Key Vault for secrets and Application Insights for logs.

## 7. Known limitations and what I would change before production

- Automated tests: unit tests for the row rules, and an integration test against a real PostgreSQL.
- Background processing: replace `Task.Run` with a queue (e.g. a hosted service), and resume imports after a restart.
- Performance: the importer queries the database per row; for the historic file (~1M rows), load approved values per vote and use bulk inserts.
- Title comparison should ignore extra spaces.
- Progress refreshed by polling every 2 s; SignalR would push it from the server.
- No real authentication: the user is chosen in the UI and sent with the request.
- Uploaded files are stored in a local folder; in production, Blob Storage.
- A uniform error format for the whole API (ProblemDetails).

## 8. Next phase with four developers

1. **Import & data quality**: performance on the historic file, validation rules, tests
2. **Review & history**: decision screens, history timeline per vote
3. **Snapshots & scoring**: positions, weights, snapshot calculation and publication
4. **Frontend & platform**: Angular screens, Docker, CI, Azure deployment

Standards first: API contract (OpenAPI + error format), naming and folder structure,
migrations workflow, code review and a minimum test coverage on business rules.