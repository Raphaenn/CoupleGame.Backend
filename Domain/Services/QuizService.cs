using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public static class QuizService
{
    public static void FillQuestionsDetails(Quiz quiz, Question question)
    {
        quiz.AddQuestion(question);
    }

    public static void AnsweredQuestions(Quiz quiz, Answers answer)
    {
        quiz.AddAnswer(answer);
    }
}