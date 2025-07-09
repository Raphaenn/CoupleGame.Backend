using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class UserRepository : IUserRepository
{
    private readonly PostgresConnection _postgresConnection;

    public UserRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }

    public async Task<User> CreateUser(User userData)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO users (id, name, email, password, birthdate, height, weight) VALUES (@id, @name, @email, @password, @birthdate, @height, @weight)";
                command.Parameters.AddWithValue("@id", userData.Id);
                command.Parameters.AddWithValue("@name", userData.Name);
                command.Parameters.AddWithValue("@email", userData.Email);
                command.Parameters.AddWithValue("@password", userData.Password);
                command.Parameters.AddWithValue("@birthdate", userData.BirthDate);
                command.Parameters.AddWithValue("@height", userData.Height);
                command.Parameters.AddWithValue("@weight", userData.Weight);

                await command.ExecuteNonQueryAsync();

                return userData;
            }
        }
    }

    public async Task<User> SearchUser(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateUser(string userId)
    {
        throw new NotImplementedException();
    }
}