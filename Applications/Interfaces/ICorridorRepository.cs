public interface ICorridorRepository
{
    Task<Corridor?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Corridor?> GetByCodeAsync(string corridorCode, CancellationToken ct = default);
    Task<List<Corridor>> GetActiveAsync(CancellationToken ct = default);
    Task AddAsync(Corridor corridor, CancellationToken ct = default);
    Task UpdateAsync(Corridor corridor, CancellationToken ct = default);
}