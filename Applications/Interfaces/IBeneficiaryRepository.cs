public interface IBeneficiaryRepository
{
    Task<Beneficiary?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Beneficiary>> GetBySenderAsync(Guid senderId, CancellationToken ct = default);
    Task AddAsync(Beneficiary beneficiary, CancellationToken ct = default);
    Task UpdateAsync(Beneficiary beneficiary, CancellationToken ct = default);
}