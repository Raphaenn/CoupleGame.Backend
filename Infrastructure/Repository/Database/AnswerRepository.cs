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

    public async Task<Answers> GetAnswer(Guid id)
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
                    Guid answerId = (Guid)reader["id"];
                    Guid userId = (Guid)reader["user_id"];
                    Guid quizId = (Guid)reader["quiz_id"];
                    string answer1 = (string)reader["answer_1"];
                    string? answer2 = (string)reader["answer_2"];
                    string? answer3 = (string)reader["answer_3"];
                    string? answer4 = (string)reader["answer_4"];
                    string? answer5 = (string)reader["answer_5"];
                    string? answer6 = (string)reader["answer_6"];
                    DateTime createdAt = (DateTime)reader["created_at"];

                    // Answers response = new Answers
                    // {
                    //     Id = answerId,
                    //     UserId = userId,
                    //     QuizId = quizId,
                    //     Answer1 = answer1,
                    //     Answer2 = answer2,
                    //     Answer3 = answer3,
                    //     Answer4 = answer4,
                    //     Answer5 = answer5,
                    //     Answer6 = answer6,
                    //     CreatedAt = createdAt
                    // };
                    // return response;
                }
            }

            throw new Exception(message: "Query error");
        }
    }

    public async Task<Answers?> GetAnswerByQuizId(Guid id)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM answers WHERE quiz_id = @id";
                
                command.Parameters.AddWithValue("@id", id);

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Guid answerId = (Guid)reader["id"];
                    Guid userId = (Guid)reader["user_id"];
                    Guid quizId = (Guid)reader["quiz_id"];
                    string answer1 = (string)reader["answer_1"];
                    string answer2 = (string)reader["answer_2"];
                    string answer3 = (string)reader["answer_3"];
                    string answer4 = (string)reader["answer_4"];
                    string answer5 = (string)reader["answer_5"];
                    string answer6 = (string)reader["answer_6"];
                    DateTime createdAt = (DateTime)reader["created_at"];

                    Answers answer = Answers.Rehydrate(answerId, userId, quizId, answer1, answer2, answer3, answer4, answer5, answer6, createdAt);
                    return answer;
                }
            }

            return null;
        }
    }

    public async Task CreateAnswer(Guid id, Guid userId, Guid quizId, string answer)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO answers (id, user_id, quiz_id, answer_1, created_at) VALUES (@id, @userId, @quizId, @answer1, @createdAt)";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@quizId", quizId);
                command.Parameters.AddWithValue("@answer1", answer);
                command.Parameters.AddWithValue("@createdAt", DateTime.Now);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateAnswer(Guid id, string answerPosition, string answer)
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