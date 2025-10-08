using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class UserRepository : IUserRepository
{
    private readonly DbSession _dbSession;
    private readonly PostgresConnection _postgresConnection;

    public UserRepository(DbSession dbSession, PostgresConnection postgresConnection)
    {
        _dbSession = dbSession;
        _postgresConnection = postgresConnection;
    }

    public async Task<User> CreateUser(User userData)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync();
        await using (var command = new NpgsqlCommand())
        {
            command.Connection = conn;
            command.CommandText = "INSERT INTO users (id, name, email, birthdate, height, weight) VALUES (@id, @name, @email, @birthdate, @height, @weight)";
            command.Parameters.AddWithValue("@id", userData.Id);
            command.Parameters.AddWithValue("@name", userData.Name);
            command.Parameters.AddWithValue("@email", userData.Email);

            await command.ExecuteNonQueryAsync();

            return userData;
        }
    }

    public async Task<User> SearchUser(Guid userId)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync();
        await using var command = new NpgsqlCommand();
        command.Connection = conn;
        command.CommandText = "SELECT * FROM users WHERE id = @userId";
        command.Parameters.AddWithValue("@userId", userId);

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Guid id = Guid.Parse(reader["id"].ToString());
            string name = reader["name"].ToString();
            string email = reader["email"].ToString();

            User user = User.Rehydrate(id, name, email);
            return user;
        }
        return null;
    }

    public async Task<List<User>> GetUserListByParams(string city)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync();
        await using var command = new NpgsqlCommand();
        command.Connection = conn;
        command.CommandText = "SELECT * FROM users WHERE city = @city";
        command.Parameters.AddWithValue("@city", city);

        var reader = await command.ExecuteReaderAsync();

        List<User> userList = new List<User>();
        while (await reader.ReadAsync())
        {
            Guid id = (Guid)reader["id"];
            string name = reader["name"].ToString() ?? string.Empty;
            string email = reader["email"].ToString() ?? string.Empty;

            User user = User.Rehydrate(id, name, email);
            userList.Add(user);
        }

        return userList;
    }

    public async Task<List<User>> GetUsersByRanking(Guid userId, string city, string sexualOrientation, int limit, CancellationToken ct)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct);
        await using var command = new NpgsqlCommand();
        command.Connection = conn;
        
        limit = Math.Clamp(limit, 1, 200);
        command.CommandText = "SELECT u.id, u.name, u.email, u.status FROM users u WHERE (@afterId IS NULL OR u.id > @afterId) AND city = @city ORDER BY u.id LIMIT @limit;";
        
        command.Parameters.AddWithValue("@afterId", userId);
        command.Parameters.AddWithValue("@city", city);
        command.Parameters.AddWithValue("@limit", limit);
        
        List<UserModel> items = new List<UserModel>(limit);
        Guid? lastId = null;

        var reader = await command.ExecuteReaderAsync(ct);

        List<User> userList = new List<User>();
        while (await reader.ReadAsync(ct))
        {
            Guid id = (Guid)reader["id"];
            string name = reader["name"].ToString() ?? string.Empty;
            string email = reader["email"].ToString() ?? string.Empty;

            User user = User.Rehydrate(id, name, email);
            userList.Add(user);
        }
        return userList;
    }

    public async Task UpdateUser(string userId)
    {
        throw new NotImplementedException();
    }
}