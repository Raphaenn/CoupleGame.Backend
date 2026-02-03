using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class QuestionRepository : IQuestionRepository
{
    private readonly PostgresConnection _postgresConnection;

    public QuestionRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }

    public async Task<Question> GetSingleQuestion(Guid questionId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM questions WHERE id = @questionId";

                command.Parameters.AddWithValue("@questionId", questionId);

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Guid id = Guid.Parse(reader["id"].ToString());
                    Guid topic = Guid.Parse(reader["topic_id"].ToString());
                    string question = reader["question"].ToString();
                    string answer1 = reader["answer_1"].ToString();
                    string answer2 = reader["answer_2"].ToString();
                    string answer3 = reader["answer_3"].ToString();
                    string answer4 = reader["answer_4"].ToString();

                    Question response = new Question(id: id, topicId: topic, questionText: question, answer1: answer1, answer2: answer2, answer3: answer3, answer4: answer4);
                    return response;
                }
            }
        }
        throw new Exception(message: "Question not found");
    }

    public async Task<List<Question>> GetQuestionsByTopicId(Guid topicId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM questions WHERE topic_id = @topicId";

                command.Parameters.AddWithValue("@topicId", topicId);

                var reader = await command.ExecuteReaderAsync();
                List<Question> questionsResult = new List<Question>();

                while (await reader.ReadAsync())
                {
                    Guid id = Guid.Parse(reader["id"].ToString());
                    Guid topic = Guid.Parse(reader["topic_id"].ToString());
                    string question = reader["question"].ToString();
                    string answer1 = reader["answer_1"].ToString();
                    string answer2 = reader["answer_2"].ToString();
                    string answer3 = reader["answer_3"].ToString();
                    string answer4 = reader["answer_4"].ToString();

                    Question response = new Question(id: id, topicId: topic, questionText: question, answer1: answer1, answer2: answer2, answer3: answer3, answer4: answer4);
                    questionsResult.Add(response);
                }

                return questionsResult;
            }
        }
    }

    public async Task<List<Question>> GetAllQuestions(CancellationToken ct)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM questions";
                
                await using var reader = await cmd.ExecuteReaderAsync(ct);
                // cache de ordinais (melhor performance)
                int ordId = reader.GetOrdinal("id");
                int ordTopic = reader.GetOrdinal("topic_id");
                int ordContent = reader.GetOrdinal("question");
                int ordAnswer1 = reader.GetOrdinal("answer_1");
                int ordAnswer2 = reader.GetOrdinal("answer_2");
                int ordAnswer3 = reader.GetOrdinal("answer_3");
                int ordAnswer4 = reader.GetOrdinal("answer_4");

                List<Question> questions = new List<Question>();
                
                while (await reader.ReadAsync(ct))
                {
                    var id       = reader.GetGuid(ordId);
                    var topic  = reader.GetGuid(ordTopic);
                    var question  = reader.GetString(ordContent);
                    var answers1 = reader.GetString(ordAnswer1);
                    var answers2    = reader.GetString(ordAnswer2);    
                    var answers3    = reader.GetString(ordAnswer3);    
                    var answers4    = reader.GetString(ordAnswer4);

                    Question q = new Question(id, topic, question, answers1, answers2, answers3, answers4);
                    questions.Add(q);

                }

                return questions;
            }
        }
    }
}