public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog, CancellationToken ct = default);
    Task<List<AuditLog>> GetByEntityAsync(
        string entityType, Guid entityId, CancellationToken ct = default);
}