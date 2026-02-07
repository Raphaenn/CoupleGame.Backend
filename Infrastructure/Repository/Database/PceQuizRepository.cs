using System.Data;
using Domain.Entities;
using Domain.Interfaces.IPce;
using Domain.Interfaces.IPCE;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class PceQuizRepository : IPceQuizRepository
{
    private readonly PostgresConnection _postgresConnection;

    public PceQuizRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }

    public async Task CreatePremiumQuiz(Pce quiz)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO pce_quiz(id, couple_id, status, created_at) VALUES (@id, @coupleId, @status, @createdAt)";

                command.Parameters.AddWithValue("@id", quiz.Id);
                command.Parameters.AddWithValue("@coupleId", quiz.CoupleId);
                command.Parameters.AddWithValue("@status", quiz.Status);
                command.Parameters.AddWithValue("@createdAt", quiz.CreatedAt);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<Pce> GetPremiumQuizByCouple(Guid coupleId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM pce_quiz WHERE couple_id = @coupleId LIMIT 1";

                command.Parameters.AddWithValue("@coupleId", coupleId);

                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);

                if (!await reader.ReadAsync())
                    return null;

                var ordId = reader.GetOrdinal("id");
                var ordCouple = reader.GetOrdinal("couple_id");
                var ordStatus = reader.GetOrdinal("status");
                var ordCreated = reader.GetOrdinal("created_at");

                Guid id = reader.GetGuid(ordId);
                Guid couple = reader.GetGuid(ordCouple);
                string status = reader.GetString(ordStatus);
                DateTime date = reader.GetDateTime(ordCreated);
                
                if (!Enum.TryParse(status, true, out PceStatus parsedStatus))
                {
                    parsedStatus = PceStatus.Pending;
                }
                

                Pce response = Pce.Rehydrate(id, couple, parsedStatus, date);

                return response;
            }
        }
    }
}