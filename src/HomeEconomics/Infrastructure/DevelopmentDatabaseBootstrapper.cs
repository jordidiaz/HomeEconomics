using Npgsql;

namespace HomeEconomics.Infrastructure;

public static class DevelopmentDatabaseBootstrapper
{
    public static async Task EnsureDatabaseExistsAsync(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var databaseName = builder.Database;

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            return;
        }

        builder.Database = "postgres";

        await using var connection = new NpgsqlConnection(builder.ConnectionString);
        await connection.OpenAsync();

        await using var checkCommand = new NpgsqlCommand(
            "SELECT 1 FROM pg_database WHERE datname = @name",
            connection);
        checkCommand.Parameters.AddWithValue("name", databaseName);
        var exists = await checkCommand.ExecuteScalarAsync();

        if (exists is not null)
        {
            return;
        }

        var escapedName = databaseName.Replace("\"", "\"\"");
        await using var createCommand = new NpgsqlCommand(
            $"CREATE DATABASE \"{escapedName}\"",
            connection);
        await createCommand.ExecuteNonQueryAsync();
    }
}
