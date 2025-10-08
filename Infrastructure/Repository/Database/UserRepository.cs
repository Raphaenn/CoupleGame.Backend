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

    public async Task<IEnumerable<User>> GetUsersByRanking(string city, string sO, int sizePlusOne, decimal? lastScore, Guid? lastId, CancellationToken ct)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct);
        await using var command = new NpgsqlCommand();
        command.Connection = conn;
        
        command.CommandText = lastScore is null || lastId is null
            ? "SELECT id, name, score, version FROM users u LEFT JOIN user_photos p ON p.user_id = u.id ORDER BY score DESC, id DESC LIMIT @limit"
            : "SELECT id, name, score, version FROM users u LEFT JOIN person_rating pr ON pr.user_id =u.id LEFT JOIN user_photos p ON p.user_id = u.id WHERE (u.score, u.id) < (@lastScore, @lastId) AND u.city = @city AND u.sexual_orientation = @sO ORDER BY score DESC, id DESC LIMIT @limit";
        
        command.Parameters.AddWithValue("@limit", sizePlusOne);
        command.Parameters.AddWithValue("@city", city);
        command.Parameters.AddWithValue("@sO", sO);
        
        if (lastScore is not null && lastId is not null)
        {
            command.Parameters.AddWithValue("lastScore", lastScore);
            command.Parameters.AddWithValue("lastId", lastId);
        }
        
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