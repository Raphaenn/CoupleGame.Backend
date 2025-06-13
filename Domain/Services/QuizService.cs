using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class QuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAnswerRepository _answerRepository;
    private readonly ICoupleRepository _coupleRepository;

    public QuizService(IQuizRepository quizRepository, IQuestionRepository questionRepository, IAnswerRepository answerRepository, ICoupleRepository coupleRepository)
    {
        _quizRepository = quizRepository;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
        _coupleRepository = coupleRepository;
    }

    public async Task<Quiz> StartQuiz(Guid coupleId, Guid questionId)
    {
        // Couples are allowed only one active quiz at a time.
        Quiz? checkByCouple = await _quizRepository.GetQuizByCoupleId(coupleId);

        if (checkByCouple != null)
        {
            throw new Exception("Couples are allowed only one active quiz at a time");
        }

        Quiz quiz = Quiz.StartQuiz(coupleId, questionId);
        
        Quiz response = await _quizRepository.StartQuiz(quiz.Id, quiz.CoupleId, quiz.Question1);
        return response;
    }

    public async Task<Quiz> UpdateQuizQuestion(Guid quizId, Guid questionId)
    {
        Quiz quiz = await _quizRepository.GetQuizById(quizId);
        
        if (quiz == null)
            throw new Exception("Quiz not found");

        bool added = quiz.Update(questionId);
        await _quizRepository.UpdateQuiz(quiz);
        return added ? quiz : null;
    }

    public async Task<Quiz> GetQuizById(Guid id)
    {
        Quiz quiz = await _quizRepository.GetQuizById(id);
        if (new Guid?[] { quiz.Question2, quiz.Question3, quiz.Question4, quiz.Question5, quiz.Question6 }
            .Any(q => q == null))
        {
            throw new Exception("Cannot get incomplete quiz");
        }
        
        List<Guid> questions = new List<Guid>();
        questions.AddRange(new Guid?[]
        {
            quiz.Question1,
            quiz.Question2,
            quiz.Question3,
            quiz.Question4,
            quiz.Question5,
            quiz.Question6,
        }.OfType<Guid>());

        IEnumerable<Task<Question>> data = questions.Select(quest => _questionRepository.GetSingleQuestion(quest));
        Question[] questionsList = await Task.WhenAll(data);

        foreach (var q in questionsList)
        {
            quiz.QuestionsDetails(q);
        }
        return quiz;
    }

    public async Task<Quiz?> GetQuizByCoupleId(Guid coupleId)
    {
        Quiz? response = await _quizRepository.GetQuizByCoupleId(coupleId);
        return response;
    }

    public async Task<Answers> AnswerAQuizQuest(Guid userId, Guid quizId, string answer)
    {
        Answers? getAnswers = await _answerRepository.GetAnswerByQuizId(quizId);

        if (getAnswers != null)
        {
            getAnswers.UpdateAnswer(answer);
            return getAnswers;
        }

        Answers newAnswers = Answers.StartAnswer(userId, quizId, answer, null, null, null, null, null);
        return newAnswers;
    }

    public async Task<Quiz> GetCompletedQuiz(Guid quizId)
    {
        Quiz quiz = await _quizRepository.GetQuizById(quizId);
        
        if (new Guid?[] { quiz.Question2, quiz.Question3, quiz.Question4, quiz.Question5, quiz.Question6 }
            .Any(q => q == null))
        {
            throw new Exception("Incomplete quiz");
        }

        // get questions
        List<Guid> questions = new List<Guid>();
        questions.AddRange(new Guid?[]
        {
            quiz.Question1,
            quiz.Question2,
            quiz.Question3,
            quiz.Question4,
            quiz.Question5,
            quiz.Question6,
        }.OfType<Guid>());   
        
        IEnumerable<Task<Question>> data = questions.Select(quest => _questionRepository.GetSingleQuestion(quest));
        Question[] questionsList = await Task.WhenAll(data);

        foreach (var q in questionsList)
        {
            quiz.QuestionsDetails(q);
        }
        
        // find couples
        Couple coupleData = await _coupleRepository.SearchCoupleById(quiz.CoupleId);
        if (coupleData.CoupleTwo == null)
        {
            throw new Exception("Incomplete couple");
        }
        
        
        // get user answers
        Task<Answers> firstUserAnswersTask = _answerRepository.GetAnswerByQuizId(coupleData.CoupleOne);
        Task<Answers> secondUserAnswersTask = _answerRepository.GetAnswerByQuizId(coupleData.CoupleTwo);
        await Task.WhenAll(firstUserAnswersTask, secondUserAnswersTask);
        Answers firstUserAnswers = await firstUserAnswersTask;
        Answers secondUserAnswers = await secondUserAnswersTask;
        
        quiz.AnsweredQuestions(firstUserAnswers);
        quiz.AnsweredQuestions(secondUserAnswers);

        return quiz;

    }
}