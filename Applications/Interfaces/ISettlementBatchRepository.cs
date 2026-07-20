public interface ISettlementBatchRepository
{
    Task<SettlementBatch?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<SettlementBatch?> GetByPartnerCorridorDateAsync(
        string partnerCode, string corridorCode, DateTime settlementDate, CancellationToken ct = default);
    Task<List<SettlementBatch>> GetPendingAsync(CancellationToken ct = default);
    Task AddAsync(SettlementBatch batch, CancellationToken ct = default);
    Task UpdateAsync(SettlementBatch batch, CancellationToken ct = default);
}