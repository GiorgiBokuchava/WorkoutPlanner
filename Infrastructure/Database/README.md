To create and seed the database, run the three migration scripts in order. From the root of this repo, execute:

```bash
cd Infrastructure/Database/Migrations

# 1. Create all base tables and schemas
sqlcmd -S .\SQLEXPRESS -E -i 001__create_tables_sqlserver.sql

# 2. Seed initial data (demo users, routines, exercises, logs, etc.)
sqlcmd -S .\SQLEXPRESS -E -i 002__seed_data_sqlserver.sql

# 3. Add authentication tables (Roles, UserRoles, RefreshTokens) and seed “Admin”/“User”
sqlcmd -S .\SQLEXPRESS -E -i 003__add_auth_tables_sqlserver.sql