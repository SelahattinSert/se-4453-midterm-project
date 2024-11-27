using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

string keyVaultUri = builder.Configuration["VaultName"];

var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

string dbHost = secretClient.GetSecret("db-url").Value.Value;
string dbUsername = secretClient.GetSecret("db-username").Value.Value;
string dbPassword = secretClient.GetSecret("db-password").Value.Value;

string connectionString = $"Host={dbHost}; Database=testdb; Port=5432; User Id={dbUsername}; Password={dbPassword}; Ssl Mode=Require;";

var app = builder.Build();

    app.MapGet("/hello", async (NpgsqlConnection dbConnection) =>
{
    try
    {
        await dbConnection.OpenAsync();
        return Results.Ok("Database connection success!");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Connection error: {ex.Message}");
    }
    finally
    {
        await dbConnection.CloseAsync();
    }
});

app.Run();

// new app feature