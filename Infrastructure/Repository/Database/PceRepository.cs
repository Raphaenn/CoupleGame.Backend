using System.Data;
using Domain.Entities;
using Domain.Interfaces.IPce;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class PceRepository : IPceRepository
{
    private readonly PostgresConnection _postgresConnection;

    public PceRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }
    
    private static void AddPceParameters(NpgsqlCommand command, Pce pce)
    {
        command.Parameters.AddWithValue("@id", pce.Id);
        command.Parameters.AddWithValue("@coupleId", pce.CoupleId);
        command.Parameters.AddWithValue("@status", (int)pce.Status);
        command.Parameters.AddWithValue("@createdAt", pce.CreatedAt);
    }

    public async Task CreatePce(Pce pce, CancellationToken ct)
    {
        const string sql = @"
        INSERT INTO pce_quiz(id, couple_id, status, created_at) 
        VALUES (@id, @coupleId, @status, @createdAt)";
    
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct);
        await using var transaction = await conn.BeginTransactionAsync(ct);
    
        try
        {
            await using var command = new NpgsqlCommand(sql, conn, transaction);
        
            AddPceParameters(command, pce);
        
            await command.ExecuteNonQueryAsync(ct);
            
            // Cancels/undoes all operations in the transaction - Restores the database to the state before the transaction started
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<Pce?> GetPceById(Guid pceId, CancellationToken ct)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct))
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM pce_quiz WHERE id = @pceId LIMIT 1";

                command.Parameters.AddWithValue("@pceId", pceId);

                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, ct);

                if (!await reader.ReadAsync(ct))
                    return null;

                var ordId = reader.GetOrdinal("id");
                var ordCouple = reader.GetOrdinal("couple_id");
                var ordStatus = reader.GetOrdinal("status");
                var ordCreated = reader.GetOrdinal("created_at");

                Guid id = reader.GetGuid(ordId);
                Guid couple = reader.GetGuid(ordCouple);
                int status = reader.GetInt32(ordStatus);
                DateTime date = reader.GetDateTime(ordCreated);
                
                var parsedStatus = status switch
                {
                    (int)PceStatus.Pending => PceStatus.Pending,
                    (int)PceStatus.Active => PceStatus.Active,
                    (int)PceStatus.Completed => PceStatus.Completed,
                    _ => PceStatus.Pending
                };

                Pce response = Pce.Rehydrate(id, couple, parsedStatus, date);

                return response;
            }
        }
    }
    
    public async Task<Pce?> GetPceByCouple(Guid coupleId, CancellationToken ct)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct))
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM pce_quiz WHERE couple_id = @coupleId LIMIT 1";

                command.Parameters.AddWithValue("@coupleId", coupleId);

                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, ct);

                if (!await reader.ReadAsync(ct))
                    return null;

                var ordId = reader.GetOrdinal("id");
                var ordCouple = reader.GetOrdinal("couple_id");
                var ordStatus = reader.GetOrdinal("status");
                var ordCreated = reader.GetOrdinal("created_at");

                Guid id = reader.GetGuid(ordId);
                Guid couple = reader.GetGuid(ordCouple);
                int status = reader.GetInt32(ordStatus);
                DateTime date = reader.GetDateTime(ordCreated);
                
                var parsedStatus = status switch
                {
                    (int)PceStatus.Pending => PceStatus.Pending,
                    (int)PceStatus.Active => PceStatus.Active,
                    (int)PceStatus.Completed => PceStatus.Completed,
                    _ => PceStatus.Pending
                };

                Pce response = Pce.Rehydrate(id, couple, parsedStatus, date);

                return response;
            }
        }
    }

    public async Task DeletePceAndData(Guid pceId, CancellationToken ct)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct);
        await using var transaction = await conn.BeginTransactionAsync(ct);
    
        try
        {
            // Delete related answers first
            await using (var command = new NpgsqlCommand(
                             "DELETE FROM pce_answer WHERE pce_id = @pceId", conn, transaction))
            {
                command.Parameters.AddWithValue("@pceId", pceId);
                await command.ExecuteNonQueryAsync(ct);
            }
        
            // Then delete the quiz
            await using (var command = new NpgsqlCommand(
                             "DELETE FROM pce_quiz WHERE id = @id", conn, transaction))
            {
                command.Parameters.AddWithValue("@id", pceId);
                await command.ExecuteNonQueryAsync(ct);
            }
        
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
    
    public async Task UpdatePceStatus(Pce pce, CancellationToken ct)
    {
        const string sql = @"UPDATE pce_quiz SET status = @status WHERE id = @id";
    
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct);
        await using var transaction = await conn.BeginTransactionAsync(ct);
    
        try
        {
            await using var command = new NpgsqlCommand(sql, conn, transaction);
        
            command.Parameters.AddWithValue("@id", pce.Id);
            command.Parameters.AddWithValue("@status", (int)pce.Status);
        
            await command.ExecuteNonQueryAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}