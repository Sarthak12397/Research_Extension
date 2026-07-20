public interface IRateApprovalLogRepository
{
    
Task<List<RateApprovalLog>> GetByCorridorRateAsync(Guid corridorRateId, CancellationToken ct = default);
    Task AddAsync(RateApprovalLog log, CancellationToken ct = default);
}