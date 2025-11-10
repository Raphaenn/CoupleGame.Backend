using System.Collections.Concurrent;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class InteractionAppService : IInteractionAppService
{
    private readonly IInteractionsRepository _interactionsRepository;
    private readonly IUserRepository _userRepository;

    public InteractionAppService(IInteractionsRepository interactionsRepository, IUserRepository userRepository)
    {
        _interactionsRepository = interactionsRepository;
        _userRepository = userRepository;
    }

    public async Task CreateUsersInteraction(string actorId, string targetId, string type)
    {
        // type = view like, pass, superlike, block
        Guid parsedActorId = Guid.Parse(actorId);
        Guid parsedTargetId = Guid.Parse(targetId);
        Interactions interaction = Interactions.CreateInteractions(parsedActorId, parsedTargetId, type);
        await _interactionsRepository.UsersInteraction(interaction);
    }

    // public async Task<IReadOnlyList<InteractionDto>> ListUserInteractions(string userId, string type, string? lastId, int sizePlusOne, CancellationToken ct)
    // {
    //     if (!Guid.TryParse(userId, out var actorId))
    //         return Array.Empty<InteractionDto>();
    //     
    //     Guid parsedUserId = Guid.Parse(userId);
    //     Guid? parsedLastId = Guid.TryParse(lastId, out var g) ? g : null;
    //
    //     IReadOnlyList<Interactions> interactions = await _interactionsRepository.ListUserInteractionsByType(parsedUserId, type, parsedLastId, sizePlusOne + 1, ct).ConfigureAwait(false);
    //     
    //     if (interactions.Count == 0) return Array.Empty<InteractionDto>();
    //
    //     List<InteractionDto> interactionList = new List<InteractionDto>();
    //     foreach (var interaction in interactions)
    //     {
    //         User users = await _userRepository.SearchUser(interaction.TargetId);
    //         InteractionDto i = new InteractionDto
    //         {
    //             Id = interaction.Id.ToString(),
    //             ActorId = interaction.ActorId.ToString(),
    //             TargetId = interaction.TargetId.ToString(),
    //             Type = interaction.Type,
    //             UserName = users.Name,
    //             UserEmail = users.Email
    //         };
    //         interactionList.Add(i);
    //     }
    //
    //     return interactionList;
    // }
    
    public async Task<IReadOnlyList<InteractionDto>> ListUserInteractions(string userId, string type, string? lastId, int sizePlusOne, CancellationToken ct)
    {
        if (!Guid.TryParse(userId, out var actorId))
            return Array.Empty<InteractionDto>();
        
        Guid parsedUserId = Guid.Parse(userId);
        Guid? parsedLastId = Guid.TryParse(lastId, out var g) ? g : null;
    
        IReadOnlyList<Interactions> interactions = await _interactionsRepository.ListUserInteractionsByType(parsedUserId, type, parsedLastId, sizePlusOne + 1, ct).ConfigureAwait(false);
        
        if (interactions.Count == 0) return Array.Empty<InteractionDto>();
        
        // Distinct dos targets
        var targetIds = interactions.Select(i => i.TargetId).Distinct().ToArray();
    
        // Limite de paralelismo para não saturar o pool/conexão
        var sem = new SemaphoreSlim(8); // ajuste conforme pool
        var userMap = new ConcurrentDictionary<Guid, User?>();
    
        var tasks = targetIds.Select(async id =>
        {
            await sem.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                var u = await _userRepository.SearchUser(id).ConfigureAwait(false);
                userMap[id] = u is null ? null : User.Rehydrate(u.Id, u.Name, u.Email, 0, 0, DateTime.Now);
            }
            finally { sem.Release(); }
        });
    
        await Task.WhenAll(tasks).ConfigureAwait(false);
    
        var list = new List<InteractionDto>(interactions.Count);
        foreach (var i in interactions)
        {
            userMap.TryGetValue(i.TargetId, out var u);
            list.Add(new InteractionDto
            {
                Id = i.Id.ToString(),
                ActorId = i.ActorId.ToString(),
                TargetId = i.TargetId.ToString(),
                Type = i.Type,
                UserName = u.Name,
                UserEmail = u.Email
            });
        }
        return list;
    }
}