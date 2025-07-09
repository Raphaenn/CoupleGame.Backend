namespace Api.Requests;

public class UserRequest
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public DateTime DataNascimento { get; set; }
    public double Altura { get; set; }
    public double Peso { get; set; }
    
}