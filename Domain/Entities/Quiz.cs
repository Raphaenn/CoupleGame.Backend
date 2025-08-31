using System.Collections.ObjectModel;

namespace Domain.Entities;

public enum QuizStatus
{
    Active,
    Pending,
    Done
}

public class Quiz
{
    public Guid Id { get; private set; }
    public Guid CoupleId { get; private set; }
    public Guid Question1 { get; private set; }
    public Guid? Question2 { get; private set; }
    public Guid? Question3 { get; private set; }
    public Guid? Question4 { get; private set; }
    public Guid? Question5 { get; private set; }
    public Guid? Question6 { get; private set; }
    public QuizStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public decimal? Result { get; set; } 
    
    // aggregate
    private readonly List<Answers> _answersList = new List<Answers>();
    public ReadOnlyCollection<Answers> AnswersList => _answersList.AsReadOnly();
    
    private readonly List<Question> _questionsList = new List<Question>();
    public ReadOnlyCollection<Question> QuestionsList => _questionsList.AsReadOnly();
    

    private Quiz(Guid id, Guid coupleId, Guid question1, Guid? question2, Guid? question3, Guid? question4, Guid? question5, Guid? question6, QuizStatus status, DateTime createdAt)
    {
        Id = id;
        CoupleId = coupleId;
        Question1 = question1;
        Question2 = question2;
        Question3 = question3;
        Question4 = question4;
        Question5 = question5;
        Question6 = question6;
        Status = status;
        CreatedAt = createdAt;
    }

    public static Quiz StartQuiz(Guid coupleId, Guid question1)
    {
        return new Quiz(Guid.NewGuid(), coupleId, question1, null, null, null, null, null, QuizStatus.Pending, DateTime.Now);
    }

    public static Quiz Rehydrate(Guid id, Guid coupleId, Guid question1, Guid? question2, Guid? question3, Guid? question4, Guid? question5, Guid? question6, QuizStatus status, DateTime createdAt)
    {
        return new Quiz(id, coupleId, question1, question2, question3, question4, question5, question6, status, createdAt);
    }

    public bool Update(Guid? questionId)
    {
        if (Question2 == null) { Question2 = questionId; return true; }
        if (Question3 == null) { Question3 = questionId; return true; }
        if (Question4 == null) { Question4 = questionId; return true; }
        if (Question5 == null) { Question5 = questionId; return true; }
        if (Question6 == null) { Question6 = questionId; return true; }
        return false;
    }
    
    public void AddQuestion(Question question)
    {
        if (question == null)
            throw new ArgumentNullException(nameof(question));

        _questionsList.Add(question);
    }
    
    internal void AddAnswer(Answers answer)
    {
        if (answer == null)
            throw new ArgumentNullException(nameof(answer));

        _answersList.Add(answer);
    }

    public static bool QuizContainsQuestion(Quiz quiz, string question)
    {
        if (!Guid.TryParse(question, out Guid questionId))
            return false;

        return quiz.Question1 == questionId
               || quiz.Question2 == questionId
               || quiz.Question3 == questionId
               || quiz.Question4 == questionId
               || quiz.Question5 == questionId
               || quiz.Question6 == questionId;
    }

    public void UpdateQuizStatus(QuizStatus newStatus)
    {
        this.Status = newStatus;
    }
}