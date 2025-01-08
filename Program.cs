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

builder.Services.AddTransient<NpgsqlConnection>(sp =>
{
    return new NpgsqlConnection(connectionString);
});

var app = builder.Build();

// /hello endpoint'i
app.MapGet("/hello", async (IServiceProvider serviceProvider) =>
{
    using var dbConnection = serviceProvider.GetRequiredService<NpgsqlConnection>();
    try
    {
        await dbConnection.OpenAsync();
        return Results.Ok("Database connection success!");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Connection error: {ex.Message}");
    }
});

app.Run();

// new app feature