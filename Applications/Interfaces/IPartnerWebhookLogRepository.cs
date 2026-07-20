public interface IPartnerWebhookLogRepository
{
    Task<PartnerWebhookLog?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<PartnerWebhookLog>> GetUnprocessedAsync(CancellationToken ct = default);
    Task AddAsync(PartnerWebhookLog log, CancellationToken ct = default);
    Task UpdateAsync(PartnerWebhookLog log, CancellationToken ct = default);
}