namespace Api.Requests;

public class UserPreferencesRequest
{
    public string? UserId { get; set; }
    public string? Localizacao { get; set; }
    public double? Distancia { get; set; }
    public string? Genero { get; set; }
    public int? IdadeMinima { get; set; }
    public int? IdadeMaxima { get; set; }
    public double? PesoMinimo { get; set; }
    public double? PesoMaximo { get; set; }
    public double? AlturaMinima { get; set; }
    public double? AlturaMaxima { get; set; }
    public List<string>? Interesses { get; set; }
}