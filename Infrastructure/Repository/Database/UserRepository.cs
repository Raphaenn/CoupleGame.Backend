using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class UserRepository : IUserRepository
{
    private readonly DbSession _dbSession;

    public UserRepository(DbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<User> CreateUser(User userData)
    {
        var conn = await _dbSession.GetConnectionAsync();
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

    public async Task<User> SearchUser(string userId)
    {
        var conn = await _dbSession.GetConnectionAsync();
        await using var command = new NpgsqlCommand();
        command.Connection = conn;
        command.CommandText = "SELECT * FROM users WHERE id == @userId";
        command.Parameters.AddWithValue("@userId", userId);

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Guid id = Guid.Parse(reader["id"].ToString());
            string name = reader["name"].ToString();
            string email = reader["birthdate"].ToString();

            User user = User.Rehydrate(id, name, email);
            return user;
        }
        return null;
    }

    public async Task UpdateUser(string userId)
    {
        throw new NotImplementedException();
    }
}