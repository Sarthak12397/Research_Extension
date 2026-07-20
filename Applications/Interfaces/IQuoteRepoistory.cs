public interface IQuoteRepository
{
    Task<Quote?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Quote?> GetByReferenceAsync(string quoteReference, CancellationToken ct = default);
    Task<List<Quote>> GetExpiredUnconvertedAsync(DateTime before, CancellationToken ct = default);
    Task AddAsync(Quote quote, CancellationToken ct = default);
    Task UpdateAsync(Quote quote, CancellationToken ct = default);
}