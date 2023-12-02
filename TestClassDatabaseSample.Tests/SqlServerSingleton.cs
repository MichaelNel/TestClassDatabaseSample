using Testcontainers.MsSql;

namespace SingletonDatabaseExampleTests;

public sealed class SqlServerSingleton
{
    private static readonly Lazy<Task<SqlServerSingleton>> Lazy = new(async () =>
    {
        var singleton = new SqlServerSingleton();
        await singleton.InitializeAsync();
        return singleton;
    });

    private readonly MsSqlContainer _container = new MsSqlBuilder().Build();

    private SqlServerSingleton()
    {
    }

    public static Task<SqlServerSingleton> Instance => Lazy.Value;

    private async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public string GetConnectionString()
    {
        return _container.GetConnectionString();
    }
}