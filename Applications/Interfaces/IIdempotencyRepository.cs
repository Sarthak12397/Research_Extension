public interface IIdempotencyRepository
{
    Task<IdempotencyRecord?> GetByKeyAsync(string idempotencyKey, CancellationToken ct = default);
    Task AddAsync(IdempotencyRecord record, CancellationToken ct = default);
}