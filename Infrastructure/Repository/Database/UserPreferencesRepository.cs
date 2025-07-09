using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Connections;
using Npgsql;

namespace Infrastructure.Repository.Database;

public class UserPreferencesRepository : IUserPreferencesRepository
{
    private readonly PostgresConnection _postgresConnection;

    public UserPreferencesRepository(PostgresConnection postgresConnection)
    {
        _postgresConnection = postgresConnection;
    }
    public async Task<UserPreferences> Create(UserPreferences preferences)
    {
        await using (var conn = await _postgresConnection.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO user_preferences  (user_id, location, distance_km, gender_preference, age_min, age_max, height_min, height_max, weight_min, weight_max, interests) VALUES (@user_id, @location, @distance_km, @gender_preference, @age_min, @age_max, @height_min, @height_max, @weight_min, @weight_max, @interests) RETURNING id";
                command.Parameters.AddWithValue("@user_id", preferences.UserId);
                command.Parameters.AddWithValue("@location", preferences.Location);
                command.Parameters.AddWithValue("@distance_km", preferences.DistanceKm ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@gender_preference", preferences.GenderPreference);
                command.Parameters.AddWithValue("@age_min", preferences.AgeMin ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@age_max", preferences.AgeMax ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@height_min", preferences.HeightMin ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@height_max", preferences.HeightMax ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@weight_min", preferences.WeightMin ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@weight_max", preferences.WeightMax ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@interests", preferences.Interests);
                var result = await command.ExecuteScalarAsync();
                preferences.Id = result != null ? (Guid)result : Guid.NewGuid();

                return preferences;
            }
        }
    }
}