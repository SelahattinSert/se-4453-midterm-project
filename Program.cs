using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

string host = "pg-db-server.postgres.database.azure.com";
string database = "testdb";
string username = "se4453pgadmin";
string password = "CCpgdbrM5U4vx4s";

string connectionString = $"Host={host}; Database={database}; Port = 5432; User Id={username}; Password={password}; Ssl Mode = Require;";

builder.Services.AddSingleton(new NpgsqlConnection(connectionString));

var app = builder.Build();

app.MapGet("/hello", async (NpgsqlConnection dbConnection) =>
{
    try
    {
        await dbConnection.OpenAsync();
        return "Database connection success!";
    }
    catch (Exception ex)
    {
        return $"Connection error: {ex.Message}";
    }
    finally
    {
        await dbConnection.CloseAsync();
    }
});

app.Run();