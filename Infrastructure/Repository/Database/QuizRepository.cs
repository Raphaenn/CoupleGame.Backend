using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class QuizRepository : IQuizRepository
{
    private readonly PostgresConnection _postgresConnection;

    public QuizRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }

    public async Task<Quiz> StartQuiz(Guid quizId, Guid coupleId, Guid questionId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO quiz (id, couple_id, question_id_1, created_at) VALUES (@id, @coupleId, @questionId, @createdAt)";
                command.Parameters.AddWithValue("@id", quizId);
                command.Parameters.AddWithValue("@coupleId", coupleId);
                command.Parameters.AddWithValue("@questionId", questionId);
                command.Parameters.AddWithValue("@createdAt", DateTime.Now);

                await command.ExecuteNonQueryAsync();

                Quiz quiz = Quiz.Rehydrate(quizId, coupleId, questionId, null, null, null, null, null, DateTime.Now);
                return quiz;
            }
        }
    }

    public async Task UpdateQuiz(Quiz quiz)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = $"UPDATE quiz SET couple_id = @coupleId, question_id_1 = @question1, question_id_2 = @question2, question_id_3 = @question3, question_id_4 = @question4, question_id_5 = @question5, question_id_6 = @question6 WHERE id = @id RETURNING *;";
                
                command.Parameters.AddWithValue("id", quiz.Id);
                command.Parameters.AddWithValue("coupleId", quiz.CoupleId);
                command.Parameters.AddWithValue("question1", (object?)quiz.Question1 ?? DBNull.Value);
                command.Parameters.AddWithValue("question2", (object?)quiz.Question2 ?? DBNull.Value);
                command.Parameters.AddWithValue("question3", (object?)quiz.Question3 ?? DBNull.Value);
                command.Parameters.AddWithValue("question4", (object?)quiz.Question4 ?? DBNull.Value);
                command.Parameters.AddWithValue("question5", (object?)quiz.Question5 ?? DBNull.Value);
                command.Parameters.AddWithValue("question6", (object?)quiz.Question6 ?? DBNull.Value);

                await command.ExecuteNonQueryAsync();
                return;
            }
        }
    }

    public async Task<Quiz> GetQuizById(Guid id)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM quiz WHERE id = @id";

                command.Parameters.AddWithValue("@id", id);

                var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                {
                    return null;
                }

                while (await reader.ReadAsync())
                {
                    Guid quizId = (Guid)reader["id"];
                    Guid couple = (Guid)reader["couple_id"];
                    Guid question1 = (Guid)reader["question_id_1"];
                    Guid? question2 = reader["question_id_2"] is DBNull ? null : (Guid)reader["question_id_2"];
                    Guid? question3 = reader["question_id_3"] is DBNull ? null : (Guid)reader["question_id_3"];
                    Guid? question4 = reader["question_id_4"] is DBNull ? null : (Guid)reader["question_id_4"];
                    Guid? question5 = reader["question_id_5"] is DBNull ? null : (Guid)reader["question_id_5"];
                    Guid? question6 = reader["question_id_6"] is DBNull ? null : (Guid)reader["question_id_6"];
                    var createdAt = reader.GetDateTime(reader.GetOrdinal("created_at"));

                    Quiz response = Quiz.Rehydrate(quizId, couple, question1, question2, question3, question4, question5, question6, createdAt);
                    return response;
                }
            }
            return null;
        }
    }

    public async Task CreateQuiz(Quiz quiz)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO quiz (id, couple_id, question_id_1, question_id_2, question_id_3, question_id_4, question_id_5, question_id_6, created_at) VALUES (@id, @coupleId, @question1, @question2, @question3, @question4, @question5, @question6, @createdAt)";

                command.Parameters.AddWithValue("@id", quiz.Id);
                command.Parameters.AddWithValue("@coupleId", quiz.CoupleId);
                // command.Parameters.AddWithValue("@question1", quiz.QuestionId1);
                // command.Parameters.AddWithValue("@question2", quiz.QuestionId2);
                // command.Parameters.AddWithValue("@question3", quiz.QuestionId3);
                // command.Parameters.AddWithValue("@question4", quiz.QuestionId4);
                // command.Parameters.AddWithValue("@question5", quiz.QuestionId5);
                // command.Parameters.AddWithValue("@question6", quiz.QuestionId6);
                command.Parameters.AddWithValue("@createAt", quiz.CreatedAt);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<Quiz?> GetQuizByCoupleId(Guid coupleId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM quiz WHERE couple_id = @coupleId";

                command.Parameters.AddWithValue("@coupleId", coupleId);

                var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                {
                    return null;
                }

                while (await reader.ReadAsync())
                {
                    Guid quizId = (Guid)reader["id"];
                    Guid couple = (Guid)reader["couple_id"];
                    Guid question1 = (Guid)reader["question_id_1"];
                    Guid question2 = (Guid)reader["question_id_2"];
                    Guid question3 = (Guid)reader["question_id_3"];
                    Guid question4 = (Guid)reader["question_id_4"];
                    Guid question5 = (Guid)reader["question_id_5"];
                    Guid question6 = (Guid)reader["question_id_6"];
                    DateTime createdAt = (DateTime)reader["created_at"];

                    // Quiz response = new Quiz
                    // {
                    //     Id = quizId,
                    //     CoupleId = couple,
                    //     QuestionId1 = question1,
                    //     QuestionId2 = question2,
                    //     QuestionId3 = question3,
                    //     QuestionId4 = question4,
                    //     QuestionId5 = question5,
                    //     QuestionId6 = question6,
                    //     CreatedAt = createdAt
                    // };
                    // return response;
                }
            }
            return null;
        }
    }
}