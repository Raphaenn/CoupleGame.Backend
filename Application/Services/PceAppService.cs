using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IPce;

namespace Application.Services;

public class PceAppService : IPceAppServices
{
    private readonly IPceRepository _pceRepository;
    private readonly IPceAnswersRepository _pceAnswersRepository;
    private readonly ICoupleRepository _coupleRepository;
    private readonly ITopicRepository _topicRepository;

    public PceAppService(IPceAnswersRepository pceAnswersRepository, IPceRepository pceRepository, ICoupleRepository coupleRepository, ITopicRepository topicRepository)
    {
        _pceAnswersRepository = pceAnswersRepository;
        _pceRepository = pceRepository;
        _coupleRepository = coupleRepository;
        _topicRepository = topicRepository;
    }

    public async Task SaveAnswers(Guid userId, Guid quizId, Guid questionId, Guid topicId, string content, CancellationToken ct)
    {
        PceAnswer answer = PceAnswer.StartAnswer(userId, quizId, questionId, topicId, content);
        await _pceAnswersRepository.CreatePceAnswer(answer, ct);
    }

    public async Task<PceDto?> GetPceByCouple(Guid coupleId, CancellationToken ct)
    {
        Pce? pce = await _pceRepository.GetPceByCouple(coupleId, ct);
        
        if (pce is null)
            return null;
        
        return new PceDto
        {
            Id = pce.Id,
            CoupleId = pce.CoupleId,
            Status = pce.Status.ToString(),
            CreatedAt = pce.CreatedAt
        };
    }

    public async Task<List<PceResultDto>> GetPceResult(Guid coupleId, CancellationToken ct)
    {
        Pce? pce = await _pceRepository.GetPceByCouple(coupleId, ct);

        if (pce is null)
        {
            throw new NotFoundException("Pce no found to this couple id");
        }
        
        Couple coupleData = await _coupleRepository.SearchCoupleById(coupleId);

        // List<Guid> userIds = new List<Guid>();
        // userIds.Add(coupleData.CoupleOne);
        // userIds.Add(coupleData.CoupleTwo);
        if (!coupleData.CoupleTwo.HasValue)
            throw new InvalidOperationException("Invalid couple");
        
        var userIds = new[] { coupleData.CoupleOne, coupleData.CoupleTwo.Value };

        var answersTask = Task.WhenAll(userIds.Select(id => _pceAnswersRepository.ListPceAnswer(id, ct)));
        Task<List<Topic>> topicsTask = _topicRepository.ListAllTopics();
        await Task.WhenAll(answersTask, topicsTask);
        
        List<PceAnswer>[] answers = await answersTask;
        List<Topic> topics = await topicsTask;
        
        // create a pcr result list to each topic
        List<PceResult> resultList = topics.Select(t => new PceResult
        {
            Id = Guid.NewGuid(),
            PceId = pce.Id,
            TopicId = t.Id,
            TopicName = t.Name
        }).ToList();
        
        // get all answer from answers array
        List<PceAnswer> allAnswers = answers.SelectMany(a => a).ToList();
        
        // group all answers bu topic
        Dictionary<Guid, List<PceAnswer>> answersByTopic = allAnswers
            .GroupBy(a => a.TopicId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var result in resultList)
        {
            if (!answersByTopic.TryGetValue(result.TopicId, out var topicAnswers))
                continue;

            foreach (var ta in topicAnswers)
            {
                result.AddPceAnswers(ta);
            }
        }
        // var answerTask = new List<Task<List<PceAnswer>>>();
        // var seen = new HashSet<(Guid userId, Guid topicId)>();
        //
        // foreach (var answerList in answers)
        // {
        //     foreach (var answer in answerList)
        //     {
        //         if (answer.TopicId == topicX)
        //         {
        //             var key = (answer.UserId, answer.TopicId); // ajuste conforme seu modelo
        //
        //             if (seen.Add(key))
        //             {
        //                 answerTask.Add(_pceAnswersRepository.ListPceAnswer(answer.UserId, ct));
        //             }
        //         }
        //     }
        // }
        
        // foreach (var result in resultList)
        // {
        //     if (!answersByTopic.TryGetValue(result.TopicId, out var topicAnswers))
        //         continue;
        //
        //     // Exemplo de regras — ajuste conforme seu domínio
        //     result.TotalQuestions = topicAnswers
        //         .Select(a => a.QuestionId)
        //         .Distinct()
        //         .Count();
        //
        //     result.AnsweredByBoth = topicAnswers
        //         .GroupBy(a => a.QuestionId)
        //         .Count(g => g.Count() == 2);
        //
        //     result.MatchPercentage = result.TotalQuestions == 0
        //         ? 0
        //         : (decimal)result.AnsweredByBoth / result.TotalQuestions * 100;
        // }
        
        // var fetchedAnswers = await Task.WhenAll(tasks); // List<PceAnswer>[]
        //
        // var resultList = fetchedAnswers
        //     .SelectMany(x => x)
        //     .Select(a => new PceResult
        //     {
        //         Id = Guid.NewGuid(),
        //         QuizId = pce.Id,
        //         TopicId = a.TopicId,
        //         QuestionId = a.QuestionId
        //     })
        //     .ToList();

        List<PceResultDto> res = new List<PceResultDto>();

        return res;
    }
}