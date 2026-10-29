# Flora Backend

## Lokal konfiguration (User Secrets)

Projektfilen (`FloraBackend.csproj`) innehåller ett gemensamt `UserSecretsId`.
För att köra backend lokalt och köra migreringar behöver du ställa in dina lokala värden (din lokala databas/connectionString).

### Steg för att konfigurera:

1. Högerklicka på `FloraBackend`-projektet i Visual Studio och välj **Manage User Secrets**.
2. Klistra in strukturen från `secrets.json.example` och anpassa värdena efter din lokala miljö:


{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FloraDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Frontend_Domain": "http://localhost:5173"
}