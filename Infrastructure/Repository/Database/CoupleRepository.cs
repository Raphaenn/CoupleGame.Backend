using System.Data;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class CoupleRepository : ICoupleRepository
{
    private readonly PostgresConnection _postgresConnection;
    
    public static object DbValue<T>(T? value) where T : struct
        => value.HasValue ? value.Value : DBNull.Value;

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
                command.CommandText = "INSERT INTO couple (id, couple_one, couple_two, type, status, created_at) VALUES (@id, @coupleOne, @coupleTwo, @type, @status, @createdAt)";
                command.Parameters.AddWithValue("@id", couple.Id);
                command.Parameters.AddWithValue("@coupleOne", couple.CoupleOne);
                command.Parameters.AddWithValue(
                    "@coupleTwo",
                    DbValue(couple.CoupleTwo)
                );
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

    public async Task<List<Couple>> SearchCoupleByUserId(Guid userId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM couple WHERE couple_one = @userId OR couple_two = @userId";;
                command.Parameters.AddWithValue("@userId", userId);

                var reader = await command.ExecuteReaderAsync();
                
                List<Couple> coupleList = new List<Couple>();
                
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
                    coupleList.Add(couple);
                }

                return coupleList;
            }
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

    public async Task<Couple?> SearchTemCouple(Guid userOneId, Guid userIdTwo)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM couple WHERE (couple_one = @coupleOne AND couple_two = @coupleTwo) OR (couple_one = @coupleTwo AND couple_two = @coupleOne) AND type = 'Temporary'";
                command.Parameters.AddWithValue("@coupleOne", userOneId);
                command.Parameters.AddWithValue("@coupleTwo", userIdTwo);

                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
                
                if (!await reader.ReadAsync())
                    return null;
                
                var ordId = reader.GetOrdinal("id");
                var ordCouple1 = reader.GetOrdinal("couple_one");
                var ordCouple2 = reader.GetOrdinal("couple_two");
                var ordType = reader.GetOrdinal("type");
                var ordStatus = reader.GetOrdinal("status");
                var ordCreated = reader.GetOrdinal("created_at");

                Guid id = reader.GetGuid(ordId);
                Guid userOne = reader.GetGuid(ordCouple1);
                Guid userTwo = reader.GetGuid(ordCouple2);
                string type = reader.GetString(ordType);
                string status = reader.GetString(ordStatus);
                DateTime date = reader.GetDateTime(ordCreated);
                
                if (!Enum.TryParse(status, true, out CoupleStatus parsedStatus))
                {
                    parsedStatus = CoupleStatus.Active;
                }
                
                if (!Enum.TryParse(type, true, out CoupleTypes parsedType))
                {
                    parsedType = CoupleTypes.Friends;
                }

                Couple couple = Couple.Rehydrate(id, userOne, userTwo, parsedType, parsedStatus, date);
                return couple;

            }
        }
    }

    public async Task<Couple?> GetLongTermCouple(Guid userId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM couple WHERE (couple_one = @userId OR couple_two = @userId) AND type IN ('Dating', 'Married')";
                command.Parameters.AddWithValue("@userId", userId);

                var reader = await command.ExecuteReaderAsync();
                
                if (!await reader.ReadAsync())
                {
                    return null;
                }
                
                var typeString = reader.GetString(reader.GetOrdinal("type"));
                var statusString = reader.GetString(reader.GetOrdinal("status"));

                if (!Enum.TryParse<CoupleTypes>(typeString, ignoreCase: true, out var type))
                {
                    throw new InvalidOperationException($"Invalid CoupleType: {typeString}");
                }

                if (!Enum.TryParse<CoupleStatus>(statusString, ignoreCase: true, out var status))
                {
                    throw new InvalidOperationException($"Invalid CoupleStatus: {statusString}");
                }

                return Couple.Rehydrate(
                    id: reader.GetGuid(reader.GetOrdinal("id")),
                    coupleOne: reader.GetGuid(reader.GetOrdinal("couple_one")),
                    coupleTwo: reader.IsDBNull(reader.GetOrdinal("couple_two"))
                        ? null
                        : reader.GetGuid(reader.GetOrdinal("couple_two")),
                    type: type,
                    status: status,
                    createdAt: reader.GetDateTime(reader.GetOrdinal("created_at"))
                );
            }
        }
    }
}