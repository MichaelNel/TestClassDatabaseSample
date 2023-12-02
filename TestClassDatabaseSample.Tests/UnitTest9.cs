using Dapper;
using Microsoft.Data.SqlClient;
using Shouldly;
using Xunit.Abstractions;

namespace SingletonDatabaseExampleTests;

public class UnitTest9 : IClassFixture<IntegrationTestFixture>
{
    private readonly SqlConnection _connection;
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest9(IntegrationTestFixture integrationTestFixture, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _connection = new SqlConnection(integrationTestFixture.DatabaseConnectionString);
    }

    [Fact]
    public async Task Test1()
    {
        _testOutputHelper.WriteLine(_connection.ConnectionString);
        var count = await _connection.ExecuteAsync("INSERT Fruit(Name) VALUES (@fruitName)", new[]
        {
            new { FruitName = "Apple" },
            new { FruitName = "Banana" },
            new { FruitName = "Pear" }
        });
        count.ShouldBe(3);
    }
}