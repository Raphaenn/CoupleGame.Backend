// using Domain.Services;
// using Domain.ValueObjects;
//
// namespace Application.UseCases;
//
// public sealed class PairingUseCase
// {
//     private readonly IPairingPolicy _policy;
//     public PairingUseCase(IPairingPolicy policy) => _policy = policy;
//
//     public Task<(Guid A, Guid B)> GetPairAsync(LadderId ladderId)
//         => _policy.PickPairAsync(ladderId);
// }