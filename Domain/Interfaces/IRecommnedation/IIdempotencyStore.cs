namespace Domain.Interfaces.IRecommnedation;

public interface IIdempotencyStore
{
    // Pode usar advisory lock transacional ou chave única no BD
    Task<IDisposable> AcquireAsync(string key, CancellationToken ct);
}