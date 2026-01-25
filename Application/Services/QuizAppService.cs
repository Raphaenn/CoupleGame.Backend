using System.Diagnostics;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;

namespace Application.Services;

public class QuizAppService : IQuizAppService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAnswerRepository _answerRepository;
    private readonly ICoupleRepository _coupleRepository;
    private readonly ITopicRepository _topicRepository;

    public QuizAppService(IQuizRepository quizRepository, IQuestionRepository questionRepository, IAnswerRepository answerRepository, ICoupleRepository coupleRepository, ITopicRepository topicRepository)
    {
        _quizRepository = quizRepository;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
        _coupleRepository = coupleRepository;
        _topicRepository = topicRepository;
    }

    public async Task<QuizDto> StartQuiz(string coupleId, string questionId)
    {
        Guid parsedCoupleId = Guid.Parse(coupleId);
        Guid parsedQuestionId = Guid.Parse(questionId);
        Quiz? checkByCouple = await _quizRepository.GetQuizByCoupleId(parsedCoupleId);

        if (checkByCouple != null)
        {
            throw new Exception("Couples are allowed only one active quiz at a time");
        }

        Quiz quiz = Quiz.StartQuiz(parsedCoupleId, parsedQuestionId);
        await _quizRepository.StartQuiz(quiz.Id, quiz.CoupleId, quiz.Question1, QuizStatus.Active);
        
        return new QuizDto
        {
            QuizId = quiz.Id.ToString(),
            CoupleId = quiz.CoupleId.ToString(),
            QuestionId1 = quiz.Question1.ToString(),
            QuestionId2 = quiz.Question2.ToString(),
            QuestionId3 = quiz.Question3.ToString(),
            QuestionId4 = quiz.Question4.ToString(),
            QuestionId5 = quiz.Question5.ToString(),
            QuestionId6 = quiz.Question6.ToString(),
            CreatedAt = quiz.CreatedAt
        };
    }

    public async Task<QuizDto?> UpdateQuiz(string quizId, string questionId)
    {
        try
        {
            Guid parsedQuizId = Guid.Parse(quizId);
            Guid parsedQuestionId = Guid.Parse(questionId);
            Quiz quiz = await _quizRepository.GetQuizById(parsedQuizId);
        
            if (quiz == null)
                throw new Exception("Quiz not found");

            bool added = quiz.Update(parsedQuestionId);
            await _quizRepository.UpdateQuiz(quiz);

            QuizDto parsedQuiz = new QuizDto
            {
                QuizId = quiz.Id.ToString(),
                CoupleId = quiz.CoupleId.ToString(),
                QuestionId1 = quiz.Question1.ToString(),
                QuestionId2 = quiz.Question2.ToString(),
                QuestionId3 = quiz.Question3.ToString(),
                QuestionId4 = quiz.Question4.ToString(),
                QuestionId5 = quiz.Question5.ToString(),
                QuestionId6 = quiz.Question6.ToString(),
                CreatedAt = quiz.CreatedAt
            };
            
            return added ? parsedQuiz : null;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<QuizDto>> ListOpenQuiz(string userId)
    {
        try
        {
            Guid parsedId = Guid.Parse(userId);
            List<Couple> couples = await _coupleRepository.SearchCoupleByUserId(parsedId);
            if (!couples.Any())
            {
                throw new InvalidOperationException($"Couple not find {parsedId}");
            }

            foreach (var couple in couples)
            {
                List<Quiz> quiz = await _quizRepository.ListQuizByCoupleId(couple.Id);

                return quiz.Select(q => new QuizDto
                    {
                        QuizId = q.Id.ToString(),
                        CoupleId = q.CoupleId.ToString(),
                        QuestionId1 = q.Question1.ToString(),
                        QuestionId2 = q.Question2.ToString(),
                        QuestionId3 = q.Question3.ToString(),
                        QuestionId4 = q.Question4.ToString(),
                        QuestionId5 = q.Question5.ToString(),
                        QuestionId6 = q.Question6.ToString(),
                        Status = q.Status.ToString(),
                        CreatedAt = q.CreatedAt
                    })
                    .ToList();
            }

            return null;
        }
        catch (Exception e)
        {
            throw new ApplicationException(e.Message);
        }
    }

    public async Task<QuizDto> GetInviteQuiz(string quizId)
    {
        try
        {
            Guid parsedQuizId = Guid.Parse(quizId);
            Quiz quiz = await _quizRepository.GetQuizById(parsedQuizId);
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
                QuizService.FillQuestionsDetails(quiz, q);
            }
            
            QuizDto parsedQuiz = new QuizDto
            {
                QuizId = quiz.Id.ToString(),
                CoupleId = quiz.CoupleId.ToString(),
                QuestionId1 = quiz.Question1.ToString(),
                QuestionId2 = quiz.Question2.ToString(),
                QuestionId3 = quiz.Question3.ToString(),
                QuestionId4 = quiz.Question4.ToString(),
                QuestionId5 = quiz.Question5.ToString(),
                QuestionId6 = quiz.Question6.ToString(),
                CreatedAt = quiz.CreatedAt,
                Questions = quiz.QuestionsList.Select(q => new QuestionDto()
                {
                    Id = q.Id.ToString(),
                    TopicId = q.TopicId.ToString(),
                    QuestionText = q.QuestionText,
                    Answer1 = q.Answer1,
                    Answer2 = q.Answer2,
                    Answer3 = q.Answer3,
                    Answer4 = q.Answer4
                }).ToList()
            };
            return parsedQuiz;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<AnswerDto> AnswerQuizQuestion(string userId, string quizId, string answer)
    {
        try
        {
            Guid parsedUserId = Guid.Parse(userId);
            Guid parsedQuizId = Guid.Parse(quizId);
            Answers? getAnswers = await _answerRepository.GetAnswerByQuizId(parsedQuizId);

            if (getAnswers != null)
            {
                var answersArray = new[]
                {
                    getAnswers.Answer1,
                    getAnswers.Answer2,
                    getAnswers.Answer3,
                    getAnswers.Answer4,
                    getAnswers.Answer5,
                    getAnswers.Answer6,
                };

                int count = answersArray.TakeWhile(a => a != null).Count();
                string position = "";
                switch (count)
                {
                    case 0: position = "answer1"; break;
                    case 1: position = "answer2"; break;
                    case 2: position = "answer3"; break;
                    case 3: position = "answer4"; break;
                    case 4: position = "answer5"; break;
                    case 5: position = "answer6"; break;
                }
            
                getAnswers.UpdateAnswer(answer);
                await _answerRepository.UpdateAnswer(getAnswers.Id, position, answer);
                AnswerDto answerDto = new AnswerDto 
                {
                    Id = getAnswers.Id.ToString(),
                    UserId = getAnswers.UserId.ToString(),
                    QuizId = getAnswers.QuizId.ToString(),
                    Answer1 = getAnswers.Answer1,
                    Answer2 = getAnswers.Answer2,
                    Answer3 = getAnswers.Answer3,
                    Answer4 = getAnswers.Answer4,
                    Answer5 = getAnswers.Answer5,
                    Answer6 = getAnswers.Answer6,
                    CreatedAt = getAnswers.CreatedAt
                };
                return answerDto;
            }

            Answers newAnswers = Answers.StartAnswer(parsedUserId, parsedQuizId, answer, null, null, null, null, null);
            await _answerRepository.CreateAnswer(newAnswers);
            // return newAnswers;
            AnswerDto newAnswerDto = new AnswerDto 
            {
                Id = newAnswers.Id.ToString(),
                UserId = newAnswers.UserId.ToString(),
                QuizId = newAnswers.QuizId.ToString(),
                Answer1 = newAnswers.Answer1,
                Answer2 = newAnswers.Answer2,
                Answer3 = newAnswers.Answer3,
                Answer4 = newAnswers.Answer4,
                Answer5 = newAnswers.Answer5,
                Answer6 = newAnswers.Answer6,
                CreatedAt = newAnswers.CreatedAt
            };
            return newAnswerDto;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<QuizDto> CompletedQuiz(string quizId)
    {
        try
        {
            Guid parsedId = Guid.Parse(quizId);
            Quiz quiz = await _quizRepository.GetQuizById(parsedId);
        
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
                QuizService.FillQuestionsDetails(quiz, q);
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
        
            QuizService.AnsweredQuestions(quiz, firstUserAnswers);
            QuizService.AnsweredQuestions(quiz, secondUserAnswers);
            
            QuizDto parsedQuiz = new QuizDto
            {
                QuizId = quiz.Id.ToString(),
                CoupleId = quiz.CoupleId.ToString(),
                QuestionId1 = quiz.Question1.ToString(),
                QuestionId2 = quiz.Question2.ToString(),
                QuestionId3 = quiz.Question3.ToString(),
                QuestionId4 = quiz.Question4.ToString(),
                QuestionId5 = quiz.Question5.ToString(),
                QuestionId6 = quiz.Question6.ToString(),
                CreatedAt = quiz.CreatedAt,
                Questions = quiz.QuestionsList.Select(q => new QuestionDto()
                {
                    Id = q.Id.ToString(),
                    TopicId = q.TopicId.ToString(),
                    QuestionText = q.QuestionText,
                    Answer1 = q.Answer1,
                    Answer2 = q.Answer2,
                    Answer3 = q.Answer3,
                    Answer4 = q.Answer4
                }).ToList(),
                Answer = quiz.AnswersList.Select(a => new AnswerDto
                {
                    Id = a.Id.ToString(),
                    UserId = a.UserId.ToString(),
                    QuizId = a.QuizId.ToString(),
                    Answer1 = a.Answer1,
                    Answer2 = a.Answer2,
                    Answer3 = a.Answer3,
                    Answer4 = a.Answer4,
                    Answer5 = a.Answer5,
                    Answer6 = a.Answer6,
                    CreatedAt = a.CreatedAt
                })
            };

            return parsedQuiz;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<QuizDto> GetResult(string quizId)
    {
        try
        {
            Stopwatch watch = Stopwatch.StartNew();
            Guid parsedQuizId = Guid.Parse(quizId);
            
            var quizTask = _quizRepository.GetQuizById(parsedQuizId);
            var answerTask = _answerRepository.ListAnswersByQuizId(parsedQuizId);

            await Task.WhenAll(quizTask, answerTask);
            Quiz quiz = await quizTask;
            List<Answers> answer = await answerTask;

            var complete = QuizService.QuizResults(quiz, answer);
            List<Guid> questions = new List<Guid>();
            questions.AddRange(new[] {
                complete.Question1,
                complete.Question2,
                complete.Question3,
                complete.Question4,
                complete.Question5,
                complete.Question6,
            }.Where(q => q.HasValue).Select(q => q.Value));

            List<Question> questionList = new List<Question>();
            foreach (var question in questions)
            {
                Question q = await _questionRepository.GetSingleQuestion(question);
                questionList.Add(q);
            }
            
            QuizDto quizDto = new QuizDto
            {
                QuizId = complete.Id.ToString(),
                CoupleId = complete.Id.ToString(),
                QuestionId1 = complete.Question1.ToString(),
                QuestionId2 = complete.Question2.ToString(),
                QuestionId3 = complete.Question3.ToString(),
                QuestionId4 = complete.Question4.ToString(),
                QuestionId5 = complete.Question5.ToString(),
                QuestionId6 = complete.Question6.ToString(),
                CreatedAt = complete.CreatedAt,
                Result = complete.Result,
                Questions = questionList.Select(qt => new QuestionDto
                {
                    Id = qt.Id.ToString(),
                    TopicId = qt.TopicId.ToString(),
                    QuestionText = qt.QuestionText,
                }),
                Answer = answer.Select(a => new AnswerDto
                {
                    Id = a.Id.ToString(),
                    UserId = a.UserId.ToString(),
                    QuizId = a.QuizId.ToString(),
                    Answer1 = a.Answer1,
                    Answer2 = a.Answer2,
                    Answer3 = a.Answer3,
                    Answer4 = a.Answer4,
                    Answer5 = a.Answer5,
                    Answer6 = a.Answer6,
                    CreatedAt = a.CreatedAt
                })
            };
            
            watch.Stop();
            Console.WriteLine($"⏱️ Tempo SEQUENCIAL: {watch.ElapsedMilliseconds} ms");
            return quizDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<QuizDto> UpdateQuizStatus(string quizId, string status)
    {
        try
        {
            Guid parsedQuizId = Guid.Parse(quizId);
            Quiz quiz = await _quizRepository.GetQuizById(parsedQuizId);

            if (quiz == null)
            {
                throw new ArgumentException("Quiz not found");
            }
            

            QuizStatus parsedStatus = (QuizStatus)Enum.Parse(typeof(QuizStatus), status);
            quiz.UpdateQuizStatus(parsedStatus);
            
            await _quizRepository.ChangeQuizStatus(quiz.Id, parsedStatus);

            QuizDto parsedQuiz = new QuizDto
            {
                QuizId = quiz.Id.ToString(),
                CoupleId = quiz.CoupleId.ToString(),
                QuestionId1 = quiz.Question1.ToString(),
                QuestionId2 = quiz.Question2.ToString(),
                QuestionId3 = quiz.Question3.ToString(),
                QuestionId4 = quiz.Question4.ToString(),
                QuestionId5 = quiz.Question5.ToString(),
                QuestionId6 = quiz.Question6.ToString(),
                Status = status,
                CreatedAt = quiz.CreatedAt
            };
            return parsedQuiz;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<QuizDto>> ListQuizByCoupleId(string coupleId)
    {
        try
        {
            Guid parsedCoupleId = Guid.Parse(coupleId);
            List<Quiz> list = await _quizRepository.ListQuizByCoupleId(parsedCoupleId);

            List<QuizDto> parsedList = new List<QuizDto>();
            foreach (var quiz in list)
            {
                parsedList.Add(new QuizDto
                {
                    QuizId = quiz.Id.ToString(),
                    CoupleId = quiz.CoupleId.ToString(),
                    QuestionId1 = quiz.Question1.ToString(),
                    QuestionId2 = quiz.Question2.ToString(),
                    QuestionId3 = quiz.Question3.ToString(),
                    QuestionId4 = quiz.Question4.ToString(),
                    QuestionId5 = quiz.Question5.ToString(),
                    QuestionId6 = quiz.Question6.ToString(),
                    CreatedAt = quiz.CreatedAt
                });
            }

            return parsedList;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    public async Task<List<QuizDto>> ListCompletedQuizByCoupleId(string coupleId)
    {
        try
        {
            Guid parsedCoupleId = Guid.Parse(coupleId);
            List<Quiz> list = await _quizRepository.ListCompletedQuizzesByCoupleId(parsedCoupleId);

            List<QuizDto> parsedList = new List<QuizDto>();
            foreach (var quiz in list)
            {
                parsedList.Add(new QuizDto
                {
                    QuizId = quiz.Id.ToString(),
                    CoupleId = quiz.CoupleId.ToString(),
                    QuestionId1 = quiz.Question1.ToString(),
                    QuestionId2 = quiz.Question2.ToString(),
                    QuestionId3 = quiz.Question3.ToString(),
                    QuestionId4 = quiz.Question4.ToString(),
                    QuestionId5 = quiz.Question5.ToString(),
                    QuestionId6 = quiz.Question6.ToString(),
                    CreatedAt = quiz.CreatedAt
                });
            }

            return parsedList;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<QuizStatsDto> GetQuizStats(string quizId)
    {
        Stopwatch watch = Stopwatch.StartNew();
        Guid parsedQuizId = Guid.Parse(quizId);
        var quizTask = _quizRepository.GetQuizById(parsedQuizId);
        var answerTask = _answerRepository.ListAnswersByQuizId(parsedQuizId);

        await Task.WhenAll(quizTask, answerTask);
        Quiz quiz = await quizTask;
        List<Answers> answers = await answerTask;

        List<Guid> questionIds = quiz.GetType()
            .GetProperties()
            .Where(p => p.Name.StartsWith("Question"))
            .Select(p => p.GetValue(quiz))
            .OfType<Guid>()
            .ToList();
        
        List<string> answersContent1 = answers[0].GetType()
            .GetProperties()
            .Where(p => p.Name.StartsWith("Answer"))
            .Select(p => p.GetValue(answers[0]))
            .OfType<string>()
            .ToList();

        List<string> answersContent2 = answers[1].GetType().GetProperties().Where(p => p.Name.StartsWith("Answer")).Select(p => p.GetValue(answers[1])).OfType<string>().ToList();
        
        foreach (var q in questionIds)
        {
            Question quest = await _questionRepository.GetSingleQuestion(q); 
            quiz.AddQuestion(quest);
        }

        // get user 1 worksheet1
        List<TopicStats> worksheet1 = new List<TopicStats>();
        foreach (var (a, q) in answersContent1.Zip(quiz.QuestionsList))
        {
            TopicStats ts = new TopicStats
            {
                TopicId = q.TopicId,
                Answer = a
            };

            worksheet1.Add(ts);
        }

        List<TopicStats> worksheet2 = new List<TopicStats>();
        foreach (var (a, q) in answersContent2.Zip(quiz.QuestionsList))
        {
            TopicStats ts = new TopicStats
            {
                TopicId = q.TopicId,
                Answer = a
            };

            worksheet2.Add(ts);
        }

        IEnumerable<IGrouping<Guid?, TopicStats>> parsed1 = worksheet1.GroupBy(t => t.TopicId).ToList();
        IEnumerable<IGrouping<Guid?, TopicStats>> parsed2 = worksheet2.GroupBy(t => t.TopicId).ToList();
        
        // var keys1 = parsed1
        //     .SelectMany(g => g.Select(t => (g.Key, t.Ans)))
        //     .ToHashSet();
        //
        // var keys2 = parsed2
        //     .SelectMany(g => g.Select(t => (g.Key, t.Ans)))
        //     .ToHashSet();
        
        // var comuns = keys1.Intersect(keys2);
        // var diferentes = keys1.Except(keys2);
        
        Dictionary<Guid?, decimal> percentages = TopicCompatibilityService.CompareByTopicPercent(parsed1, parsed2);

        // topics
        Dictionary<string, string> topics = new Dictionary<string, string>()
        {
            {"e6128121-b023-4683-b20d-91126aa22c9c", "Finance"},
            {"6ba6a519-9090-4534-90ba-06c7ed8506c2", "Religion"},
            {"82382bc1-8213-428a-9ccd-cd9be564451c", "Fidelity" },
            {"bb386817-bbcf-454c-bece-9d779088514c", "Sex"},
            {"bc2e707d-adb9-482e-ae5f-3b5c87a72879", "Work"},
            {"f0ab27e6-ec6a-4d1f-b8c3-3224211831b7", "Home"},
        };
        
        QuizStatsDto res = new QuizStatsDto
        {
            QuizId = quiz.Id,
            Finance = 0,
            Sex = 0,
            Fidelity = 0,
            Work = 0,
            Religion = 0,
            Home = 0
        };
        foreach (var (topicId, percent) in percentages)
        {
            topics.TryGetValue(topicId.ToString(), out var value);
            
            if (value == "Finance") res.Finance = percent;
            if (value == "Work") res.Work = percent;
            if (value == "Sex") res.Sex = percent;
            if (value == "Home") res.Home = percent;
            if (value == "Fidelity") res.Fidelity = percent;
            if (value == "Religion") res.Religion = percent;

        }
        
        watch.Stop();
        Console.WriteLine($"⏱️ Tempo SEQUENCIAL: {watch.ElapsedMilliseconds} ms");
        return res;
    }
}