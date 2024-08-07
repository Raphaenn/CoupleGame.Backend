using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class AnswerRepository : IAnswerRepository
{
    private readonly PostgresConnection _postgresConnection;

    public AnswerRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }

    public async Task<Answers> GetAnswer(string id)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM answers WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    string answerId = (string)reader["id"];
                    string userId = (string)reader["user_id"];
                    string quizId = (string)reader["quiz_id"];
                    string answer1 = (string)reader["answer_1"];
                    string? answer2 = (string)reader["answer_2"];
                    string? answer3 = (string)reader["answer_3"];
                    string? answer4 = (string)reader["answer_4"];
                    string? answer5 = (string)reader["answer_5"];
                    string? answer6 = (string)reader["answer_6"];
                    DateTime createdAt = (DateTime)reader["created_at"];

                    Answers response = new Answers
                    {
                        Id = answerId,
                        UserId = userId,
                        QuizId = quizId,
                        Answer1 = answer1,
                        Answer2 = answer2,
                        Answer3 = answer3,
                        Answer4 = answer4,
                        Answer5 = answer5,
                        Answer6 = answer6,
                        CreatedAt = createdAt
                    };
                    return response;
                }
            }

            throw new Exception(message: "Query error");
        }
    }

    public async Task CreateAnswer(string id, string userId, string quizId, string answer)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO answers (id, user_ID, quiz_id, answer, created_at) VALUES (@id, @userId, @quizId, @answer, @createdAt)";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@user_id", userId);
                command.Parameters.AddWithValue("@quiz_id", quizId);
                command.Parameters.AddWithValue("@answer_1", answer);
                command.Parameters.AddWithValue("@createdAt", DateTime.Now);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateAnswer(string id, string answerPosition, string answer)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = $"UPDATE answers SET {answerPosition} = @answer WHERE id = @id RETURNING id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@answer", answer);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}