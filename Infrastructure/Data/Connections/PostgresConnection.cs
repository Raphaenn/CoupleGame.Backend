using Npgsql;

namespace Infrastructure.Data.Connections;
public class PostgresConnection : IAsyncDisposable
{
    private readonly NpgsqlDataSource _connection;

    public PostgresConnection(string connection)
    {
        _connection = NpgsqlDataSource.Create(connection);;
    }
    
    public NpgsqlDataSource DataSource => _connection;

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}