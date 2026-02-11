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
    
    public async Task<PceDto> InitNewPce(Guid coupleId, CancellationToken ct)
    {
        // todo: check if already exists a valid pce
        Pce pce = Pce.StartPce(coupleId);
        await _pceRepository.CreatePce(pce, ct);
        PceDto parsedPce = new PceDto
        {
            Id = pce.Id,
            CoupleId = pce.CoupleId,
            Status = pce.Status.ToString(),
            CreatedAt = pce.CreatedAt
        };

        return parsedPce;
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
    
    public async Task SaveAnswers(Guid userId, Guid pceId, Guid questionId, Guid topicId, string content, CancellationToken ct)
    {
        Pce? pce = await _pceRepository.GetPceById(pceId, ct);
        if (pce == null)
        {
            throw new Exception("Invalid pce");
        }
        PceAnswer answer = PceAnswer.StartAnswer(userId, pceId, questionId, topicId, content);
        await _pceAnswersRepository.CreatePceAnswer(answer, ct);

        pce.ChangeStatus(PceStatus.Active);
        await _pceRepository.UpdatePceStatus(pce, ct);
    }

    public async Task<List<PceResultDto>> GetPceResult(Guid pceId, CancellationToken ct)
    {
        Pce? pce = await _pceRepository.GetPceById(pceId, ct);

        if (pce is null)
        {
            throw new NotFoundException("Pce no found to this couple id");
        }
        
        Couple coupleData = await _coupleRepository.SearchCoupleById(pce.CoupleId);

        if (!coupleData.CoupleTwo.HasValue)
            throw new InvalidOperationException("Invalid couple");
        
        var userIds = new[] { coupleData.CoupleOne, coupleData.CoupleTwo.Value };

        // parallel query
        var answersTask = Task.WhenAll(userIds.Select(id => _pceAnswersRepository.ListPceAnswer(id, ct)));
        Task<List<Topic>> topicsTask = _topicRepository.ListAllTopics();
        await Task.WhenAll(answersTask, topicsTask);
        
        // his and her answers array
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

        
        List<PceResultDto> pceResultDto = new List<PceResultDto>();
        foreach (var pceData in resultList)
        {
            PceResultDto dto = new PceResultDto
            {
                Id = pceData.Id,
                PceId = pceData.PceId,
                TopicId = pceData.TopicId,
                TopicName = pceData.TopicName,
                Questions = pceData.PceAnswers
                    .GroupBy(a => a.QuestionId)
                    .Select(g => new PceQuestionAnswersDto
                    {
                        QuestionId = g.Key,
                        Answers = g.Select(a => new PceAnswerDto
                        {
                            UserId = a.UserId,
                            Content = a.Content, 
                            QuestionId = a.QuestionId,
                            CreatedAt = a.CreatedAt
                        }).ToList()
                    })
                    .ToList()
            };
            pceResultDto.Add(dto);
        }

        return pceResultDto;
    }
    
    public async Task DeleteCompletePce(Guid pceId, CancellationToken ct)
    {
        // todo: check if already exists a valid pce
        await _pceRepository.DeletePceAndData(pceId, ct);
    }

}