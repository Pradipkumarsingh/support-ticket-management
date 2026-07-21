# Database Setup Notes

## Choice and Version

- **Database**: Microsoft SQL Server (LocalDB or Developer/Express edition)
- **Recommended local instance**: `(localdb)\MSSQLLocalDB`

## Connection String Example

Use the `DefaultConnection` entry in `appsettings.json` in `SupportTicketManagement.Api`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\\\MSSQLLocalDB;Database=SupportTicketDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Update the server or database name as needed for your environment, but **do not** commit real passwords or production connection strings.

## Schema / Migrations

- The `database/schema-or-migrations/initial-schema.sql` script creates the core tables:
  - `Users`
  - `Tickets`
  - `Comments`
- Run it once against your SQL Server database:

```sql
-- From SQL Server Management Studio or sqlcmd
USE SupportTicketDb;
GO
-- Execute the contents of initial-schema.sql
```

If you prefer Entity Framework Core migrations, you can also generate and apply them locally with:

```powershell
dotnet ef migrations add InitialCreate `
  -p src/backend/SupportTicketManagement.Api/SupportTicketManagement.Api.csproj `
  -s src/backend/SupportTicketManagement.Api/SupportTicketManagement.Api.csproj

dotnet ef database update `
  -p src/backend/SupportTicketManagement.Api/SupportTicketManagement.Api.csproj `
  -s src/backend/SupportTicketManagement.Api/SupportTicketManagement.Api.csproj
```

(These commands modify your local database only; they are not run automatically.)

## Seed Data

- The `database/seed-data/seed-data.sql` script inserts:
  - Three users: `Alice Admin`, `Bob Support`, `Charlie User`
  - Two example tickets
  - One example comment

Run after creating the schema:

```sql
USE SupportTicketDb;
GO
-- Execute the contents of seed-data.sql
```

## Verifying Persistence

1. Start SQL Server and create/apply schema + seed.
2. Run the backend API:
   - `cd ai-practical-assessment/src/backend/SupportTicketManagement.Api`
   - `dotnet run`
3. Use the Angular frontend to:
   - List tickets (you should see the seeded tickets).
   - Create a new ticket and add a comment.
4. Stop the API and Angular dev server.
5. Restart the API and frontend and reload the ticket list:
   - All tickets and comments should still be present, confirming persistence across restarts.

