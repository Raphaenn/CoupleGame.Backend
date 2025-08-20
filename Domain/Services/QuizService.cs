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

    public static Quiz QuizResults(Quiz quiz, List<Answers> answer)
    {
        if (answer[0].QuizId != answer[1].QuizId)
        {
            throw new ArgumentException("Answers must be from the same quiz");
        }

        decimal matchCount = 0;        
        if (answer[0].Answer1 == answer[1].Answer1) matchCount++;
        if (answer[0].Answer2 == answer[1].Answer2) matchCount++;
        if (answer[0].Answer3 == answer[1].Answer3) matchCount++;
        if (answer[0].Answer4 == answer[1].Answer4) matchCount++;
        if (answer[0].Answer5 == answer[1].Answer5) matchCount++;
        if (answer[0].Answer6 == answer[1].Answer6) matchCount++;
        quiz.Result = matchCount / 6;

        return quiz;
    }
}