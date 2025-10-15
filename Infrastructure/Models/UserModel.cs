using System.Text.Json.Serialization;

namespace Infrastructure.Models;

public sealed record PhotoDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("url")] string Url
    );

public class UserModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public decimal Rating { get; set; }
    public List<PhotoDto> Photos { get; set; }
}