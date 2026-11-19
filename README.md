# Flora Backend

REST API for Flora, a plant-care app. It handles user accounts, plants, care tasks and care logs, plant health logs, plant images, profiles, and a plant species search through the [Perenual](https://perenual.com/) API. Frontend: [FloraFrontend](https://github.com/CountKrakula/FloraFrontend).

Built with ASP.NET Core (.NET 10), Entity Framework Core with SQL Server, and ASP.NET Core Identity for cookie-based login.

## Run it locally

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download), a SQL Server instance, and the EF Core tool (`dotnet tool install --global dotnet-ef`).

```bash
git clone https://github.com/CountKrakula/FloraBackend.git
cd FloraBackend
```

1. Set your local configuration (see below).
2. Create the database: `dotnet ef database update`
3. Trust the HTTPS dev certificate (once): `dotnet dev-certs https --trust`
4. Start the API: `dotnet run --launch-profile https`

The API runs at `https://localhost:7064`, and the interactive API docs (Scalar) are at `https://localhost:7064/scalar`. Use the HTTPS profile, because the login cookie is `Secure` and the browser only accepts it over HTTPS.

## Configuration (User Secrets)

`FloraBackend.csproj` has a shared `UserSecretsId`, so each developer keeps their own local values in .NET User Secrets, which are never committed. In Visual Studio, right-click the project and choose **Manage User Secrets**, then paste the structure from `secrets.json.example` and adjust it:

| Key | Description |
| --- | --- |
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `Frontend_Domain` | Address of the frontend, for CORS (for example `http://localhost:5173`, no trailing slash) |
| `Perenual:ApiKey` | API key for the plant species endpoints |

To get a Perenual key, create a free account at [perenual.com](https://perenual.com/) and generate one.

## Tests

```bash
dotnet test FloraBackend.sln
```

Unit tests are in `FloraBackend.Tests` (xUnit and Moq). They also run in the GitHub Actions pipeline before every deploy.

## Deployment

A push to `main` builds the project, runs the tests and deploys to Azure with GitHub Actions. If a test fails, nothing is deployed.