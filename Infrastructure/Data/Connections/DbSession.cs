using Npgsql;

namespace Infrastructure.Data.Connections;

public class DbSession : IAsyncDisposable
{
    private readonly PostgresConnection _postgresConnection;
    private NpgsqlConnection? _connection;

    public DbSession(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }

    public async Task<NpgsqlConnection> GetConnectionAsync()
    {
        if (_connection == null)
        {
            _connection = await _postgresConnection.DataSource.OpenConnectionAsync();
        }

        return _connection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
    }
}