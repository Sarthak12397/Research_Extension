public interface IComplianceCaseRepository
{
    Task<ComplianceCase?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ComplianceCase?> GetByTransactionIdAsync(Guid transactionId, CancellationToken ct = default);
    Task<List<ComplianceCase>> GetOpenAsync(CancellationToken ct = default);
    Task AddAsync(ComplianceCase complianceCase, CancellationToken ct = default);
    Task UpdateAsync(ComplianceCase complianceCase, CancellationToken ct = default);
}