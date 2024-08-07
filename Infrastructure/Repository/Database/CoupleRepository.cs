using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class CoupleRepository : ICoupleRepository
{
    private readonly PostgresConnection _postgresConnection;

    public CoupleRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }

    public async Task<Couple> CreateCouple(Couple couple)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO couple (id, couple_one, couple_two, type, status, created_at) VALUES (@id, @coupleOne, @coupleTwo, @type, @status, @createdAt)";
                command.Parameters.AddWithValue("@id", couple.Id);
                command.Parameters.AddWithValue("@coupleOne", couple.CoupleOne);
                command.Parameters.AddWithValue("@coupleTwo", couple.CoupleTwo);
                command.Parameters.AddWithValue("@type", couple.Type);
                command.Parameters.AddWithValue("@status", couple.Status);
                command.Parameters.AddWithValue("@createdAt", couple.CreatedAt);

                await command.ExecuteNonQueryAsync();

                return couple;
            }
        }
    }

    public async Task<Couple?> SearchCoupleRelationship(string userOneId, string userIdTwo)
    {
        Console.WriteLine(Guid.Parse(userOneId));
        Console.WriteLine(Guid.Parse(userIdTwo));
        Console.WriteLine("opa");
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM couple WHERE (couple_one = @coupleOne AND couple_two = @coupleTwo) OR (couple_one = @coupleTwo AND couple_two = @coupleOne)";
                command.Parameters.AddWithValue("@coupleOne", Guid.Parse(userOneId));
                command.Parameters.AddWithValue("@coupleTwo", Guid.Parse(userIdTwo));

                var reader = await command.ExecuteReaderAsync();
                Console.WriteLine("opa 2");
                
                if (!reader.HasRows)
                {
                    // Handle the case where there are no rows
                    Console.WriteLine("returning null");
                    return null; // or handle accordingly
                }

                while (await reader.ReadAsync())
                {
                    string id = (string)reader["id"];
                    string userOne = (string)reader["couple_one"];
                    string userTwo = (string)reader["couple_two"];
                    string type = (string)reader["type"];
                    string status = (string)reader["status"];
                    DateTime createdAt = (DateTime)reader["created_at"];

                    Couple couple = new Couple(id: id, coupleOne: userOne, coupleTwo: userTwo, type: type, status: status, createdAt: createdAt);
                    return couple;
                }

            }
        }
        return null;
    }
}