using Domain;
using Domain.Entities;
using Domain.Interfaces.IRecommnedation;
using Domain.ValueObjects;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class RecommendationRepository : ILadderRepository, IParticipantRatingRepository, IMatchVoteRepository
{
    
    private readonly PostgresConnection _postgresConnection;

    public RecommendationRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }

    public async Task<Ladder?> GetLadder(LadderId id, CancellationToken ct)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM ladder WHERE id = @id";
                command.Parameters.AddWithValue("@id", Guid.Parse(id.ToString()));

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Guid ladderId = (Guid)reader["id"];
                    LadderId ldId = new LadderId(ladderId);
                    Ladder ladder = new Ladder(ldId, "brazil", EloParams.Default);
                    return ladder;
                }
            }

            return null;
        }
    }

    public async Task EnsureExistsAsync(LadderId ladderId, Guid userId, CancellationToken ct)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM person_rating WHERE user_id = @userId AND ladder_id = @ladderId;";
                
                command.Parameters.AddWithValue("userId", userId);
                command.Parameters.AddWithValue("ladderId", ladderId);

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    
                }
            }
        }
    }

    public async Task<PersonRating> GetForUpdateAsync(LadderId ladderId, Guid userId, CancellationToken ct)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct))
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM person_rating WHERE user_id = @userId AND ladder_id = @ladderId;";
                
                command.Parameters.AddWithValue("userId", userId);
                command.Parameters.AddWithValue("ladderId", Guid.Parse(ladderId.ToString()));

                var reader = await command.ExecuteReaderAsync(ct);
                
                if (!reader.HasRows)
                {
                    return null;
                }

                while (await reader.ReadAsync(ct))
                {
                    Guid ladder = (Guid)reader["ladder_id"];
                    Guid user = (Guid)reader["user_id"];    
                    double rating = Convert.ToDouble(reader["rating"]);
                    int wins = (int)reader["wins"];
                    int losses = (int)reader["losses"];

                    LadderId parsedLadder = new LadderId(ladder);
                    PersonRating person = new PersonRating(parsedLadder, user, rating, wins, losses);
                    return person;
                }
            }

            return null;
        }
    }

    public async Task UpdateAsync(PersonRating rating, CancellationToken ct)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct))
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "UPDATE person_rating SET rating = @rating, wins = @wins, losses = @losses WHERE user_id = @userId;";
                
                command.Parameters.AddWithValue("userId", rating.UserId);
                command.Parameters.AddWithValue("rating", rating.Rating);
                command.Parameters.AddWithValue("wins", rating.Wins);
                command.Parameters.AddWithValue("losses", rating.Losses);
                
                await command.ExecuteNonQueryAsync(ct);
            }
        } 
    }

    public async Task<Guid> InsertAsync(MatchVote vote, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid?> GetByIdempotencyKeyAsync(string key, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}