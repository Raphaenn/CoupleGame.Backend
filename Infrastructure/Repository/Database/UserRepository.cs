using System.Text.Json;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Data.Connections;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class UserRepository : IUserRepository
{
    private readonly DbSession _dbSession;
    private readonly PostgresConnection _postgresConnection;

    public UserRepository(DbSession dbSession, PostgresConnection postgresConnection)
    {
        _dbSession = dbSession;
        _postgresConnection = postgresConnection;
    }

    public async Task<User> CreateUser(User userData)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync();
        await using (var command = new NpgsqlCommand())
        {
            command.Connection = conn;
            command.CommandText = "INSERT INTO users (id, name, email, birthdate, height, weight) VALUES (@id, @name, @email, @birthdate, @height, @weight)";
            command.Parameters.AddWithValue("@id", userData.Id);
            command.Parameters.AddWithValue("@name", userData.Name);
            command.Parameters.AddWithValue("@email", userData.Email);

            await command.ExecuteNonQueryAsync();

            return userData;
        }
    }

    public async Task<User> SearchUser(Guid userId)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync();
        await using var command = new NpgsqlCommand();
        command.Connection = conn;
        command.CommandText = "SELECT * FROM users WHERE id = @userId";
        command.Parameters.AddWithValue("@userId", userId);

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Guid id = (Guid)reader["id"];
            string name = reader["name"].ToString() ?? string.Empty;
            string email = reader["email"].ToString() ?? string.Empty;
            DateTime birthDate = (DateTime)reader["birthdate"];
            double uHeight = (double)reader["height"];
            double uWeight = (double)reader["weight"];

            User user = User.Rehydrate(id, name, email, uHeight, uWeight, birthDate);
            return user;
        }
        return null;
    }

    public async Task<IReadOnlyList<User>> GetUsersByParams(string city, string sexuality, string sexualOrientation, double? height, double? weight, int size, CancellationToken ct)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct);
        await using var command = new NpgsqlCommand();
        command.Connection = conn;

        if (height is not null || weight is not null)
        {
            command.CommandText = """
              SELECT u.id, u.name, u.email, u.birthdate, u.height, u.weight,
              COALESCE(
                json_agg(
                    json_build_object(
                        'id', p.id,
                        'url', p.url
                    )
                ) FILTER ( WHERE p.id IS NOT NULL),
                '[]'::json
              )  AS photos
              FROM users u
              INNER JOIN user_photo p ON p.user_id = u.id
              WHERE u.city = @city AND u.sexuality = @sexuality AND u.sexual_orientation = @sO AND height = @height AND weight = @weight
              GROUP BY u.id, u.name, u.email, u.birthdate, u.height, u.weight
              LIMIT @size;
              """;
            
            command.Parameters.AddWithValue("@city", city);
            command.Parameters.AddWithValue("@sexuality", sexuality);
            command.Parameters.AddWithValue("@sO", sexualOrientation);
            command.Parameters.AddWithValue("@height", height);
            command.Parameters.AddWithValue("@weight", weight);
            command.Parameters.AddWithValue("@size", size);
            
        }
        else
        {
            command.CommandText = """
              SELECT u.id, u.name, u.email, u.birthdate, u.height, u.weight,
              COALESCE(
                json_agg(
                    json_build_object(
                        'id', p.id,
                        'url', p.url
                    )
                ) FILTER ( WHERE p.id IS NOT NULL),
                '[]'::json
              )  AS photos
              FROM users u
              INNER JOIN user_photo p ON p.user_id = u.id
              WHERE u.city = @city AND u.sexuality = @sexuality AND u.sexual_orientation = @sO
              GROUP BY u.id, u.name, u.email, u.birthdate, u.height, u.weight
              LIMIT @size;
              """;
            
            command.Parameters.AddWithValue("@city", city);
            command.Parameters.AddWithValue("@sexuality", sexuality);
            command.Parameters.AddWithValue("@sO", sexualOrientation);
            command.Parameters.AddWithValue("@size", size);
        }
        
        List<User> userList = new List<User>();
        var reader = await command.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            Guid id = (Guid)reader["id"];
            string name = reader["name"].ToString() ?? string.Empty;
            string email = reader["email"].ToString() ?? string.Empty;
            DateTime birthDate = (DateTime)reader["birthdate"];
            double uHeight = reader["height"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["height"]);
            double uWeight = reader["weight"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["weight"]);

            User user = User.Rehydrate(id, name, email, uHeight, uWeight, birthDate);
            userList.Add(user);
        }

        return userList;
    }

    public async Task<IReadOnlyList<User>> GetUsersByRanking(string city, string sexuality, string sO, int sizePlusOne, decimal? lastScore, Guid? lastId, CancellationToken ct)
    {
        await using var conn = await _postgresConnection.DataSource.OpenConnectionAsync(ct);
        await using var command = new NpgsqlCommand();
        command.Connection = conn;
        
        var firstPage = lastScore is null && lastId is null;

        command.CommandText = !firstPage
            ? """
              SELECT
                u.id,
                u.name,
                u.email,
                u.birthdate,
                u.height, 
                u.weight,
                pr.rating,
                COALESCE(
                  json_agg(
                    json_build_object(
                      'id', p.id,
                      'url', p.url,
                      'created_at', p.created_at
                    )
                    ORDER BY p.created_at DESC
                  ) FILTER (WHERE p.id IS NOT NULL),
                  '[]'::json
                ) AS photos
              FROM users u
              INNER JOIN user_photo p ON p.user_id = u.id
              INNER JOIN person_rating pr ON pr.user_id = u.id
              WHERE
              (
                  pr.rating < $1
                  OR (pr.rating = $1 AND u.id < $2)
              )
              AND u.city = $3 AND u.sexual_orientation = $4 AND u.sexuality = $6
              GROUP BY u.id, u.name, u.email, u.birthdate, u.height, u.weight, pr.rating
              ORDER BY pr.rating DESC, u.id DESC
              LIMIT $5;
              """
            : """
              SELECT
                u.id,
                u.name,
                u.email,
                u.birthdate,
                u.height, 
                u.weight,
                pr.rating,
                COALESCE(
                  json_agg(
                    json_build_object(
                      'id', p.id,
                      'url', p.url,
                      'created_at', p.created_at
                    )
                    ORDER BY p.created_at DESC
                  ) FILTER (WHERE p.id IS NOT NULL),
                  '[]'::json
                ) AS photos
              FROM users u
              INNER JOIN user_photo p ON p.user_id = u.id
              LEFT JOIN person_rating pr ON pr.user_id = u.id
              WHERE u.city = @city AND u.sexual_orientation = @sO AND u.sexuality = @sexuality
              GROUP BY u.id, u.name, u.email, u.birthdate, u.height, u.weight, pr.rating
              ORDER BY pr.rating DESC, u.id DESC
              LIMIT @limit;
              """;
        
        if (firstPage)
        {
            command.Parameters.AddWithValue("city", city);
            command.Parameters.AddWithValue("sexuality", sexuality);
            command.Parameters.AddWithValue("sO", sO);
            command.Parameters.AddWithValue("limit", sizePlusOne);
        }
        else
        {
            command.Parameters.AddWithValue("$1", lastScore);
            command.Parameters.AddWithValue("$2", lastId);
            command.Parameters.AddWithValue("$3", city);
            command.Parameters.AddWithValue("$4", sO);
            command.Parameters.AddWithValue("$5", sizePlusOne);
            command.Parameters.AddWithValue("$6", sexuality);
        }
        
        List<User> userList = new List<User>();
        
        var reader = await command.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            Guid id = (Guid)reader["id"];
            string name = reader["name"].ToString() ?? string.Empty;
            string email = reader["email"].ToString() ?? string.Empty;
            DateTime birthDate = (DateTime)reader["birthdate"];
            double uHeight = reader["height"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["height"]);
            double uWeight = reader["weight"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["weight"]);

            decimal? rating = reader["rating"] is DBNull ? null : (decimal)reader["rating"];

            // A coluna 4 é JSON (texto) com o array de fotos
            string photosJson = reader.IsDBNull(reader.GetOrdinal("photos"))
                ? "[]"
                : reader.GetString(reader.GetOrdinal("photos"));

            // Deserializa para List<PhotoDto>
            var photos = JsonSerializer.Deserialize<List<PhotoDto>>(
                photosJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<PhotoDto>();

            User addedUser = User.Rehydrate(id, name, email, uHeight, uWeight, birthDate);
            addedUser.AddScore(rating ?? 1500);
            photos.ForEach(p =>
            {
                UserPhotos parsed = new UserPhotos(p.Id.ToString(), p.Url);
                addedUser.AddPhoto(parsed);
            });
            
            userList.Add(addedUser);
        }
        return userList;
    }

    public async Task UpdateUser(string userId)
    {
        throw new NotImplementedException();
    }
}