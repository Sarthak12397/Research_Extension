public interface ITransactionRepository
{
    Task<RemittanceTransaction?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<RemittanceTransaction?> GetByReferenceAsync(string referenceNumber, CancellationToken ct = default);
    Task<List<RemittanceTransaction>> GetPaidByPartnerAndDateAsync(
        string partnerCode, string corridorCode, DateTime settlementDate, CancellationToken ct = default);
    Task<decimal> GetTotalSendAmountTodayAsync(Guid senderId, CancellationToken ct = default);
    Task<decimal> GetTotalSendAmountThisMonthAsync(Guid senderId, CancellationToken ct = default);
    Task AddAsync(RemittanceTransaction transaction, CancellationToken ct = default);
    Task UpdateAsync(RemittanceTransaction transaction, CancellationToken ct = default);
}