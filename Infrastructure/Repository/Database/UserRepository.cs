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
                command.CommandText = "INSERT INTO users (id, name, email, password, birthdate) VALUES (@id, @name, @email, @password, @birthdate)";
                command.Parameters.AddWithValue("@id", userData.Id);
                command.Parameters.AddWithValue("@name", userData.Name);
                command.Parameters.AddWithValue("@email", userData.Email);
                command.Parameters.AddWithValue("@password", userData.Password);
                command.Parameters.AddWithValue("@birthdate", userData.BirthDate);

                await command.ExecuteNonQueryAsync();

                return userData;
            }
        }
    }

    public Task<User> SearchUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUser(string userId)
    {
        throw new NotImplementedException();
    }
}