# JobTrackr

A portfolio-ready ASP.NET Core MVC job application tracker. It demonstrates authentication with hashed passwords, role support, user-owned CRUD data, EF Core, repository pattern, validation, secure file upload checks, interview scheduling, and dashboard analytics.

## Run locally

```powershell
dotnet restore
dotnet run
```

Register an account and begin tracking. The app uses SQLite for a frictionless local demo. For production, replace `UseSqlite` with `UseSqlServer` and set a SQL Server connection string.
