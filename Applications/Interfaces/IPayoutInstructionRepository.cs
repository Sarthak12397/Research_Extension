public interface IPayoutInstructionRepository
{
    Task<PayoutInstruction?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PayoutInstruction?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct = default);
    Task<List<PayoutInstruction>> GetPendingByPartnerAsync(string partnerCode, CancellationToken ct = default);
    Task AddAsync(PayoutInstruction instruction, CancellationToken ct = default);
    Task UpdateAsync(PayoutInstruction instruction, CancellationToken ct = default);
}