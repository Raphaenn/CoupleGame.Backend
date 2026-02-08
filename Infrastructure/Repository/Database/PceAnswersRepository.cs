using Domain.Entities;
using Domain.Interfaces.IPce;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class PceAnswersRepository : IPceAnswersRepository
{
    private readonly PostgresConnection _postgresConnection;

    public PceAnswersRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }

    public async Task CreatePceAnswer(PceAnswer answer, CancellationToken ct)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct))
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO pce_answer(id, user_id, quiz_id, question_id, content, created_at) VALUES (@id, @userId, @quizId, @questionId, @content, @createdAt)";

                command.Parameters.AddWithValue("@id", answer.Id);
                command.Parameters.AddWithValue("@userId", answer.UserId);
                command.Parameters.AddWithValue("@quizId", answer.QuizId);
                command.Parameters.AddWithValue("@questionId", answer.QuestionId);
                command.Parameters.AddWithValue("@content", answer.Content);
                command.Parameters.AddWithValue("@createdAt", answer.CreatedAt);

                await command.ExecuteNonQueryAsync(ct);
            }
        }
    }

    public async Task<List<PceAnswer>> ListPceAnswer(Guid userId, CancellationToken ct)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct))
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT FROM pce_answer WHERE user_id = @userId";

                command.Parameters.AddWithValue("@userId", userId);

                var reader = await command.ExecuteReaderAsync(ct);

                if (!reader.HasRows)
                {
                    return null;
                }

                var ordId = reader.GetOrdinal("id");
                var ordUser = reader.GetOrdinal("user_id");
                var ordQuiz = reader.GetOrdinal("quiz_id");
                var ordQuest = reader.GetOrdinal("question_id");
                var ordTopic = reader.GetOrdinal("topic_id");
                var ordContent = reader.GetOrdinal("content");
                var ordCreateAt = reader.GetOrdinal("created_at");

                List<PceAnswer> answerList = new List<PceAnswer>();

                while (await reader.ReadAsync(ct))
                {
                    Guid id = reader.GetGuid(ordId);
                    Guid user = reader.GetGuid(ordUser);
                    Guid quiz = reader.GetGuid(ordQuiz);
                    Guid question = reader.GetGuid(ordQuest);
                    Guid topic = reader.GetGuid(ordTopic);
                    string content = reader.GetString(ordContent);
                    DateTime createdAt = reader.GetDateTime(ordCreateAt);
                    
                    PceAnswer answer = PceAnswer.Rehydrate(id, user, quiz, question, topic, content, createdAt);
                    answerList.Add(answer);
                }

                return answerList;
            }
        }
    }
}