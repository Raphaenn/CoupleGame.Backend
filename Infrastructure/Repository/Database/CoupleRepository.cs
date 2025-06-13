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

    public async Task<Couple> StartNewCouple(Couple couple)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO couple (id, couple_one, type, status, created_at) VALUES (@id, @coupleOne, @type, @status, @createdAt)";
                command.Parameters.AddWithValue("@id", couple.Id);
                command.Parameters.AddWithValue("@coupleOne", couple.CoupleOne);
                command.Parameters.AddWithValue("@type", couple.Type.ToString());
                command.Parameters.AddWithValue("@status", couple.Status.ToString());
                command.Parameters.AddWithValue("@createdAt", couple.CreatedAt);

                await command.ExecuteNonQueryAsync();

                return couple;
            }
        }
    }

    public async Task AddCoupleMember(Guid coupleId, Guid userId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "UPDATE couple SET couple_two = @coupleTwo WHERE id = @id";
                command.Parameters.AddWithValue("@id", coupleId);
                command.Parameters.AddWithValue("@coupleTwo", userId);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<Couple> SearchCoupleById(Guid coupleId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM couple WHERE id = @coupleId";
                command.Parameters.AddWithValue("@coupleId", coupleId);

                var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                {
                    return null;
                }
                while (await reader.ReadAsync())
                {
                    Guid id = (Guid)reader["id"];
                    Guid userOne = (Guid)reader["couple_one"];
                    Guid? userTwo = reader["couple_two"] is DBNull ? null : (Guid)reader["couple_two"];
                    string type = (string)reader["type"];
                    string status = (string)reader["status"];
                    DateTime createdAt = (DateTime)reader["created_at"];

                    Enum.TryParse<CoupleTypes>(type, out var parsedType);
                    Enum.TryParse<CoupleStatus>(status, out var parsedStatus);
                    Couple couple = Couple.Rehydrate(id, userOne, userTwo, parsedType, parsedStatus, createdAt);
                    return couple;
                }
            }
            return null;
        }
    }

    public async Task<Couple> SearchCoupleByUserId(Guid userId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM couples WHERE user_id = @userId";
                command.Parameters.AddWithValue("@userId", userId);

                var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                {
                    return null;
                }
                while (await reader.ReadAsync())
                {
                    Guid id = (Guid)reader["id"];
                    Guid userOne = (Guid)reader["couple_one"];
                    Guid? userTwo = reader["couple_two"] is DBNull ? null : (Guid)reader["couple_two"];
                    string type = (string)reader["type"];
                    string status = (string)reader["status"];
                    DateTime createdAt = (DateTime)reader["created_at"];
                    return null;
                }
            }

            return null;
        }
    }

    public async Task<Couple?> SearchCoupleRelationship(string userOneId, string userIdTwo)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM couple WHERE (couple_one = @coupleOne AND couple_two = @coupleTwo) OR (couple_one = @coupleTwo AND couple_two = @coupleOne)";
                command.Parameters.AddWithValue("@coupleOne", Guid.Parse(userOneId));
                command.Parameters.AddWithValue("@coupleTwo", Guid.Parse(userIdTwo));

                var reader = await command.ExecuteReaderAsync();
                
                if (!reader.HasRows)
                {
                    return null;
                }

                while (await reader.ReadAsync())
                {
                    Guid id = (Guid)reader["id"];
                    Guid userOne = (Guid)reader["couple_one"];
                    Guid userTwo = (Guid)reader["couple_two"];
                    string type = (string)reader["type"];
                    string status = (string)reader["status"];
                    DateTime createdAt = (DateTime)reader["created_at"];

                    // Couple couple = new Couple(id: id, coupleOne: userOne, coupleTwo: userTwo, type: type, status: status, createdAt: createdAt);
                    return null;
                }

            }
        }
        return null;
    }
}