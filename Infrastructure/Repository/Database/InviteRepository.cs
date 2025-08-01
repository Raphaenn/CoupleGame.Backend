using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class InviteRepository : IInviteRepository
{
    private readonly PostgresConnection _postgresConnection;

    public InviteRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }

    public async Task CreateInvite(Invite invite)
    {
        await using(var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO invite (id, quiz_id, host_user_id, email, accepted, created_at) VALUES (@id, @quizId, @hostId, @email, @accepted, @createdAt)";

                command.Parameters.AddWithValue("@id", invite.Id);
                command.Parameters.AddWithValue("@quizId", invite.QuizId);
                command.Parameters.AddWithValue("@hostId", invite.HostId);
                command.Parameters.AddWithValue("@email", invite.Email);
                command.Parameters.AddWithValue("@accepted", invite.Accepted);
                command.Parameters.AddWithValue("@createdAt", invite.CreatedAt);

                await command.ExecuteReaderAsync();
                return;
            }
        }
    }

    public async Task<Invite> GetInviteByEmail(Guid quizId, string email)
    {
        throw new NotImplementedException();
    }

    public async Task<Invite?> GetInviteById(Guid id)
    {
        await using (var connection = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT * FROM invite WHERE id = @id";
                
                command.Parameters.AddWithValue("id", id);

                var reader = await command.ExecuteReaderAsync();
                
                if (!reader.HasRows)
                {
                    return null;
                }
                while (await reader.ReadAsync())
                {
                    Guid inviteId = (Guid)reader["id"];
                    Guid quizId = (Guid)reader["quiz_id"];
                    Guid hostId = (Guid)reader["host_user_id"];
                    string email = (string)reader["email"];
                    bool accepted = (bool)reader["accepted"];
                    
                    Invite res = Invite.Rehydrate(inviteId, quizId, hostId, email, accepted);

                    return res;
                }
            }
            return null;
        }
    }

    public async Task AcceptInvite(Invite invite)
    {
        await using (var connection = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "UPDATE invite SET accepted = @accept WHERE id = @id";

                command.Parameters.AddWithValue("@id", invite.Id);
                command.Parameters.AddWithValue("@accept", invite.Accepted);
                
                await command.ExecuteNonQueryAsync();
                return;
            }
        }
    }
}