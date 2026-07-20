public interface IFeeRuleRepository
{
    Task<FeeRule?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<FeeRule?> GetActiveByIdAsync(Guid id, DateTime at, CancellationToken ct = default);
}