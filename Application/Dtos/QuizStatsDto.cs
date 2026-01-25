namespace Application.Dtos;

public class QuizStatsDto
{
    public Guid QuizId { get; set; }
    public decimal Finance { get; set; }
    public decimal Sex { get; set; }
    public decimal Fidelity { get; set; }
    public decimal Work { get; set; }
    public decimal Religion { get; set; }
    public decimal Home { get; set; }
}