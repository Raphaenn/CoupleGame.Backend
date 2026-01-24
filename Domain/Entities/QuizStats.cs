namespace Domain.Entities;

public class QuizStats
{
    public Guid QuizId { get; set; }
    public string Finance { get; set; } = String.Empty;
    public string Sex { get; set; } = String.Empty;
    public string Fidelity { get; set; } = String.Empty;
    public string Work { get; set; } = String.Empty;
    public string Religion { get; set; } = String.Empty;
    public string Home { get; set; } = String.Empty;

    public QuizStats(Guid quizId, string finance, string sex, string fidelity, string work, string religion, string home)
    {
        QuizId = quizId;
        Finance = finance;
        Sex = sex;
        Fidelity = fidelity;
        Work = work;
        Religion = religion;
        Home = home;
    }
}