# AI usage

I used Claude (Anthropic) as an assistant during the whole assignment.

How I used it:
- Understanding the assignment and the domain (imports, conflicts, snapshots)
- Designing the data model step by step, and discussing alternatives
- Generating code that I then read, adapted and tested
- Debugging (Docker, npm, EF Core errors, failing imports on the sample files)
- Drafting the README and DECISIONS.md

Where I rejected or changed its output:
- I simplified several proposals: no in-memory queue, fewer fields in Import, report counters computed from ImportRow
- I moved logic into services (VoteService, ConflictService, CsvUtils) to keep the import readable
- I rewrote the conflict decision endpoint myself as a single, simpler endpoint
- I fixed bugs it introduced or missed (duplicated title conflicts, malformed lines, wrong file paths)

Every piece of code was run and tested on the provided CSV files.