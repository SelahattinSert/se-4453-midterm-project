# SE 4453 Midterm Project

A simple ASP.NET Core application that integrates with Azure Key Vault and PostgreSQL.

---

## Features
- Fetches database credentials securely from Azure Key Vault.
- Connects to PostgreSQL using the `Npgsql` library.
- Contains a single endpoint `/hello` to test the database connection.

---

## Setup

1. **Prerequisites:**
   - Azure Key Vault with secrets:
     - `db-url`
     - `db-username`
     - `db-password`
   - PostgreSQL database.

2. **Run Locally:**
   - Set the `VaultName` in `appsettings.json` or environment variables.
   - Run the app:
     ```bash
     dotnet run
     ```

---

## License
MIT
