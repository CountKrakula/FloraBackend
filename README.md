# Flora Backend

## Lokal konfiguration (User Secrets)

Projektfilen (`FloraBackend.csproj`) innehåller ett gemensamt `UserSecretsId`. 
För att köra backend lokalt, köra migreringar och anropa externa tjänster behöver du ställa in dina lokala värden.

### Steg för att konfigurera:

1. Högerklicka på **FloraBackend**-projektet i Visual Studio och välj **Manage User Secrets**.
2. Klistra in strukturen från `secrets.json.example` och anpassa värdena efter din lokala miljö (t.ex. connection string och din Perenual API-nyckel).

---

### 🔑 Perenual API-nyckel (för PlantSpecies)

För att testa eller utveckla mot `PlantSpecies`-endpoints krävs en API-nyckel från **Perenual**:

1. Skapa ett gratis konto på [Perenual](https://perenual.com/) och generera en API-nyckel.
2. Lägg till nyckeln i din lokala `secrets.json` under `"Perenual": { "ApiKey": "DIN_API_NYCKEL" }`.