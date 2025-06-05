using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class TopicRepository : ITopicRepository
{
    private readonly PostgresConnection _postgresConnection;

    public TopicRepository(PostgresConnection postgresConnection)
    {
        this._postgresConnection = postgresConnection;
    }

    public async Task<Topic?> GetTopicById(Guid id)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM topic WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                {
                    return null;
                }

                while (await reader.ReadAsync())
                {
                    Guid topicId = Guid.Parse(reader["id"].ToString());
                    string name = reader["name"].ToString() ?? string.Empty;
                    string description = (string)reader["description"];
                    bool status = (bool)reader["status"];
                    
                    Topic topicResponse = new Topic(id: topicId, name: name, description: description, status: status);

                    return topicResponse;
                }
            }
        }
        return null;
    }

    public async Task<List<Topic>> ListAllTopics()
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM topic WHERE status = true";

                var reader = await command.ExecuteReaderAsync();
                List<Topic> topics = new List<Topic>();

                while (await reader.ReadAsync())
                {
                    Guid questionId = Guid.Parse(reader["id"].ToString());
                    string topicaName = reader["name"].ToString() ?? string.Empty;
                    string description = (string)reader["description"];
                    bool status = (bool)reader["status"];
                    
                    Topic topicResponse = new Topic(id: questionId, name: topicaName, description: description, status: status);
                    topics.Add(topicResponse);
                }
                return topics;
            }
        }
    }

    public async Task<Topic> GetRandomQuestionByTopic(Guid topicId)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM topic WHERE status = true";
                command.Parameters.AddWithValue("@id", topicId);
                
                var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    Guid questionId = Guid.Parse(reader["id"].ToString() ?? string.Empty);
                    string topicaName = reader["name"].ToString() ?? string.Empty;
                    string description = (string)reader["description"];
                    bool status = (bool)reader["status"];
                    
                    Topic topicResponse = new Topic(id: questionId, name: topicaName, description: description, status: status);
                return topicResponse;
                };
            }
        }
        return null;
    }
}