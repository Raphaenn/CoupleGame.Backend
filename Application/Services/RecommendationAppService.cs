using Application.Dtos;
using Application.Interfaces;
// using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRecommnedation;
using Domain.Services;
using Domain.ValueObjects;

namespace Application.Services;

public class RecommendationAppService : IRecommendationAppService
{
    private readonly ILadderRepository _ladderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IParticipantRatingRepository _recommendationRepository;

    public RecommendationAppService(ILadderRepository ladderRepository, IUserRepository userRepository, IParticipantRatingRepository recommendationRepository)
    {
        _ladderRepository = ladderRepository;
        _userRepository = userRepository;
        _recommendationRepository = recommendationRepository;
    }

    public async Task<CursorPage<UserDto>> GetRecommendationService(string city, string sexuality, string sexualOrientation, int size, RankingCursor? after, CancellationToken ct)
    {
        Task<IReadOnlyList<User>> usersByRanking = _userRepository.GetUsersByRanking(city, sexuality, sexualOrientation, sizePlusOne: size - 4, lastScore: after?.LastScore, lastId: after?.LastId, ct);
        Task<IReadOnlyList<User>> usersByParams = _userRepository.GetUsersByParams(city, sexuality, sexualOrientation, null, null, sizePlusOne: size - 6, ct);
        
        /*  Factory
         * Transform the repository requests into params list
         * Rules: 70% com base no Elo (exploit)
           20% perfis novos/recém-criados (explore)
           10% aleatórios controlados para não engessar
         */
        
        IReadOnlyList<User>[] results = await Task.WhenAll(usersByParams, usersByRanking);
        var usersByParamsResult  = results[0];
        var usersByRankingResult = results[1];

        var parsedRes = usersByParamsResult.Concat(usersByRankingResult).DistinctBy(u => u.Id).ToList();
        
        // var pageItems = (hasNext ? usersByRanking.Take(size) : usersByRanking).ToList();
        //
        // var next = hasNext
        //     ? new RankingCursor(
        //         LastScore: pageItems[^1].Score,
        //         LastId:    pageItems[^1].Id)
        //     : null;

        RankingCursor cursor = new RankingCursor(after?.LastScore, after?.LastId, parsedRes.Count);
        
        return new CursorPage<UserDto>(
            parsedRes.Select(u => new UserDto
            {
                Id = u.Id.ToString(),
                Name = u.Name,
                Email = u.Email,
                Rating = u.Score,
                Photos = u.Photos.Select(p => p.Url).ToList()
            }).ToList(),
            cursor);
    }

    public async Task<IEnumerable<PersonRating>> SimulateRecommendationService(LadderId ladderId, int size, CancellationToken ct)
    {
        IEnumerable<User> users = await _userRepository.GetUsersByParams("Niterói", "Male", "Heterosexual", null, null, 10, ct);
        List<PersonRating> pRatingList = new List<PersonRating>();
        // MatchVote vote = new MatchVote();
        List<PersonRating> ranking = new List<PersonRating>();
        
        foreach (var user in users)
        {
            PersonRating p = new PersonRating(ladderId, user.Id);
            pRatingList.Add(p);
        }
        // get random match
        var picker = new RandomPairingPolicy(pRatingList);

        // vote simulate
        for (int i = 0; i < 20; i++)
        {
            var (p1, p2) = picker.PickPairAsync();
            
            var win = (new Random().NextDouble() < 0.6)           // Randomly decide bias path.
                ? (string.Compare(p1.UserId.ToString(), p2.UserId.ToString(), StringComparison.Ordinal) <= 0 ? p1 : p2)
                : (string.Compare(p1.UserId.ToString(), p2.UserId.ToString(), StringComparison.Ordinal) > 0  ? p1 : p2);
            
            var param = new EloParams(KFactor: 32, Scale: 400, AllowDraws: false);
            
            // derive o outcome com base em quem venceu
            var outcome = ReferenceEquals(win, p1) ? InteractionType.Like : InteractionType.Dislike;

            var svc = new EloRatingResult();
            EloResult r = svc.Apply(p1, p2, outcome, param);
        }

        return pRatingList.GroupBy(p => p.UserId).Select(o => o.First()).OrderByDescending(r => r.Rating);

        // PersonRating pRating = new PersonRating(ladderId, users)
    }

    public async Task RecordVoteService(LadderId ladderId, Guid a, Guid b, InteractionType interaction, string? idempotencyKey, CancellationToken ct)
    {
        PersonRating p1 = await _recommendationRepository.GetForUpdateAsync(ladderId, a, ct);
        PersonRating p2 = await _recommendationRepository.GetForUpdateAsync(ladderId, b, ct);
        
        Console.WriteLine(p1.Rating);
        var param = new EloParams(KFactor: 32, Scale: 400, AllowDraws: false);
        var svc = new EloRatingResult();
        
        EloResult r = svc.Apply(p1, p2, interaction, param);
        Console.WriteLine($"Updated rating: {r.ABefore}");
        Console.WriteLine(p1.Rating);
       
        // save results on DB
        await _recommendationRepository.UpdateAsync(p1, ct);
        // await _recommendationRepository.UpdateAsync(p2, ct);
    }

    public async Task? ShowRanking(LadderId ladderId)
    {
        throw new NotImplementedException();
    }

    public async Task<Ladder> GetLadderById(string id, CancellationToken ct)
    {
        LadderId newid = new LadderId(Guid.Parse(id));
        Ladder? ladder = await _ladderRepository.GetLadder(newid, ct);

        if (ladder == null)
        {
            throw new Exception("Invalid ladder id");
        }
        // ladder.PersonRatings.

        return ladder;
    }
}