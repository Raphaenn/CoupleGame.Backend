// Domain/Services/IPairingPolicy.cs

using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Services;

public interface IPairingPolicy
{
    // Decide como encontrar B dado A (ex.: por rating mais próximo),
    // mas sem acesso a SQL aqui. A implementação concreta chama repositórios.
    (PersonRating A, PersonRating B) PickPairAsync(double maxRatingGap = 200);
}