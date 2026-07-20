public interface ICorridorRateRepository
{
    Task<CorridorRate?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CorridorRate?> GetCurrentActiveRateAsync(Guid corridorId, DateTime at, CancellationToken ct = default);
    Task<List<CorridorRate>> GetPendingApprovalAsync(CancellationToken ct = default);
    Task<List<CorridorRate>> GetHistoryByCorridorAsync(Guid corridorId, CancellationToken ct = default);
    Task AddAsync(CorridorRate rate, CancellationToken ct = default);
    Task UpdateAsync(CorridorRate rate, CancellationToken ct = default);
}