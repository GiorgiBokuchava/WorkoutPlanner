To create and seed the DB:

```bash
sqlcmd -S .\SQLEXPRESS -E -i database\01_create_tables.sql
sqlcmd -S .\SQLEXPRESS -E -i database\02_seed_data.sql