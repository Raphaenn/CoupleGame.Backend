using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class InteractionRepository : IInteractionsRepository
{
    private readonly PostgresConnection _postgresConnection;

    public InteractionRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }
    
    public async Task UsersInteraction(Interactions interaction)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync();
        await using (var command = new NpgsqlCommand())
        {
            command.Connection = conn;
            command.CommandText = """
              INSERT INTO interactions (id, actor_id, target_id, type, created_at)
              VALUES (@id, @actorId, @targetId, @type, @createdAt)
              ON CONFLICT (@actorId, @targetId, @type) DO NOTHING
              """;


            command.Parameters.AddWithValue("@id", interaction.Id);
            command.Parameters.AddWithValue("@actorId", interaction.ActorId);
            command.Parameters.AddWithValue("@targetId", interaction.TargetId);
            command.Parameters.AddWithValue("@type", interaction.Type);
            command.Parameters.AddWithValue("@createdAt", DateTime.Now);

            await command.ExecuteNonQueryAsync();
        }
    }
}