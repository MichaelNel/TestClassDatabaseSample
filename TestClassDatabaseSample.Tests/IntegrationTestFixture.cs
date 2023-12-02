using DatabaseMigrations;
using DbUp;
using Microsoft.Data.SqlClient;

namespace SingletonDatabaseExampleTests;

public class IntegrationTestFixture : IAsyncLifetime
{
    public string? DatabaseConnectionString;

    public async Task InitializeAsync()
    {
        var sqlServerSingleton = await SqlServerSingleton.Instance;
        var serverConnectionString = sqlServerSingleton.GetConnectionString();
        DatabaseConnectionString = new SqlConnectionStringBuilder(serverConnectionString)
        {
            InitialCatalog = $"integration-test-{Guid.NewGuid()}"
        }.ConnectionString;

        EnsureDatabase.For.SqlDatabase(DatabaseConnectionString);
        DeployChanges.To.SqlDatabase(DatabaseConnectionString)
            .WithScriptsEmbeddedInAssembly(typeof(Migrations).Assembly)
            .Build()
            .PerformUpgrade();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}