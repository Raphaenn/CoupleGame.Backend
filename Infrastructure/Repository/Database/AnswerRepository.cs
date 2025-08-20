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

                    return Answers.Rehydrate(
                        answerId,
                        userId,
                        quizId,
                        answer1,
                        answer2,
                        answer3,
                        answer4,
                        answer5,
                        answer6,
                        createdAt
                    );

                }
            }

            throw new Exception(message: "Query error");
        }
    }

    public async Task<Answers?> GetAnswerByQuizId(Guid? id)
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

    public async Task CreateAnswer(Answers answer)
    {
        await using (var connect = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = connect;
                command.CommandText = "INSERT INTO answers (id, user_id, quiz_id, answer_1, answer_2, answer_3, answer_4, answer_5, answer_6, created_at) VALUES (@id, @userId, @quizId, @answer1, @answer2, @answer3, @answer4, @answer5, @answer6, @createdAt)";

                command.Parameters.AddWithValue("@id", answer.Id);
                command.Parameters.AddWithValue("@userId", answer.UserId);
                command.Parameters.AddWithValue("@quizId", answer.QuizId);
                command.Parameters.AddWithValue("@answer1", answer.Answer1);
                command.Parameters.AddWithValue("answer2", (object?)answer.Answer2 ?? DBNull.Value);
                command.Parameters.AddWithValue("answer3", (object?)answer.Answer3 ?? DBNull.Value);
                command.Parameters.AddWithValue("answer4", (object?)answer.Answer4 ?? DBNull.Value);
                command.Parameters.AddWithValue("answer5", (object?)answer.Answer5 ?? DBNull.Value);
                command.Parameters.AddWithValue("answer6", (object?)answer.Answer6 ?? DBNull.Value);
                command.Parameters.AddWithValue("@createdAt", answer.CreatedAt);

                await command.ExecuteReaderAsync();
                return;
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

    public async Task<Answers> GetCompletedAnswers(Guid quizId, Guid userId)
    {
        await using (var connect = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = connect;
                command.CommandText = command.CommandText = @"SELECT 
                      a.*,
                      q1.question AS question_1,
                      q2.question AS question_2,
                      q3.question AS question_3,
                      q4.question AS question_4,
                      q5.question AS question_5,
                      q6.question AS question_6
                    FROM answers a
                    JOIN quiz qz ON a.quiz_id = qz.id
                    JOIN questions q1 ON qz.question_id_1 = q1.id
                    JOIN questions q2 ON qz.question_id_2 = q2.id
                    JOIN questions q3 ON qz.question_id_3 = q3.id
                    JOIN questions q4 ON qz.question_id_4 = q4.id
                    JOIN questions q5 ON qz.question_id_5 = q5.id
                    JOIN questions q6 ON qz.question_id_6 = q6.id
                    WHERE a.quiz_id = @quizId AND a.user_id = @userId;";

                command.Parameters.AddWithValue("@quizId", quizId);
                command.Parameters.AddWithValue("@userId", userId);

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Guid answerId = (Guid)reader["id"];
                    Guid user = (Guid)reader["user_id"];
                    Guid quiz = (Guid)reader["quiz_id"];
                    string answer1 = (string)reader["answer_1"];
                    string answer2 = (string)reader["answer_2"];
                    string answer3 = (string)reader["answer_3"];
                    string answer4 = (string)reader["answer_4"];
                    string answer5 = (string)reader["answer_5"];
                    string answer6 = (string)reader["answer_6"];
                    string question1 = (string)reader["question_1"];
                    string question2 = (string)reader["question_2"];
                    string question3 = (string)reader["question_3"];
                    string question4 = (string)reader["question_4"];
                    string question5 = (string)reader["question_5"];
                    string question6 = (string)reader["question_6"];
                    DateTime createdAt = (DateTime)reader["created_at"];

                    Answers answers = Answers.Rehydrate(answerId, user, quiz, answer1, answer2, answer3, answer4, answer5, answer6, createdAt);
                    List<string> questions = new List<string>()
                    {
                        question1,
                        question2,
                        question3,
                        question4,
                        question5,
                        question6
                    };
                    answers.AddQuestion(questions);

                    return answers;
                }
            }

            return null;
        }
    }

    public async Task<List<Answers>> ListAnswersByQuizId(Guid id)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM answers WHERE quiz_id = @id";
                
                command.Parameters.AddWithValue("@id", id);

                var reader = await command.ExecuteReaderAsync();

                List<Answers> answersList = new List<Answers>();
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
                    answersList.Add(answer);
                }
                return answersList;
            }
        }
    }

    public async Task<Answers> GetAnswersByUserId(Guid id)
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
}