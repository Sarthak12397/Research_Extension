public interface ISenderRepository
{
    Task<Sender?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Sender?> GetByDocumentHashAsync(string documentHash, CancellationToken ct = default);
    Task AddAsync(Sender sender, CancellationToken ct = default);
    Task UpdateAsync(Sender sender, CancellationToken ct = default);
}