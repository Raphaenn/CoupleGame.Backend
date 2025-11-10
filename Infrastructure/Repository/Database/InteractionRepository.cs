using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;
using NpgsqlTypes;

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
            // todo - add upsert to use idempontence
           
            command.CommandText = "INSERT INTO interactions (id, actor_id, target_id, type, created_at) VALUES (@id, @actorId, @targetId, @type, @createdAt)";

            command.Parameters.AddWithValue("@id", interaction.Id);
            command.Parameters.AddWithValue("@actorId", interaction.ActorId);
            command.Parameters.AddWithValue("@targetId", interaction.TargetId);
            command.Parameters.AddWithValue("@type", interaction.Type);
            command.Parameters.AddWithValue("@createdAt", DateTime.Now);

            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<IReadOnlyList<Interactions>> ListUserInteractionsByType(Guid actorId, string type, Guid? lastId, int limitPlusOne, CancellationToken ct)
    {
        var sql = """
          SELECT i.id, i.actor_id, i.target_id, i.type
          FROM interactions i
          WHERE i.actor_id = @actorId
            AND (@type IS NULL OR i.type = @type)
            AND (@lastId IS NULL OR i.id > @lastId)
          ORDER BY i.id
          LIMIT @limitPlusOne;
          """;

        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct);
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.Add(new NpgsqlParameter("@actorId", NpgsqlDbType.Uuid) { Value = actorId });
        cmd.Parameters.Add(new NpgsqlParameter("@type",   NpgsqlDbType.Text) { Value = (object?)type ?? DBNull.Value });
        cmd.Parameters.Add(new NpgsqlParameter("@lastId", NpgsqlDbType.Uuid) { Value = (object?)lastId ?? DBNull.Value });
        cmd.Parameters.Add(new NpgsqlParameter("@limitPlusOne",  NpgsqlDbType.Integer) { Value = limitPlusOne });

        var list = new List<Interactions>(limitPlusOne);

        await using var reader = await cmd.ExecuteReaderAsync(ct);

        // cache de ordinais (melhor performance)
        var ordId       = reader.GetOrdinal("id");
        var ordActorId  = reader.GetOrdinal("actor_id");
        var ordTargetId = reader.GetOrdinal("target_id");
        var ordType     = reader.GetOrdinal("type");

        while (await reader.ReadAsync(ct))
        {
            var id       = reader.GetGuid(ordId);
            var actor  = reader.GetGuid(ordActorId);
            var targetId = reader.GetGuid(ordTargetId);
            var iType    = reader.GetString(ordType);

            list.Add(Interactions.Rehydrate(id, actor, targetId, iType));
        }

        // nunca retorne null
        return list.ToArray(); // snapshot leve, expõe como IReadOnlyList<Interactions>
    }
}