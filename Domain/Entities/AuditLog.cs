public class AuditLog
{
    public Guid Id { get; private set; }
    public string EntityType { get; private set; }
    public Guid EntityId { get; private set; }
    public string Action { get; private set; }
    public string? BeforeValue { get; private set; }
    public string? AfterValue { get; private set; }
    public string PerformedBy { get; private set; }
    public string? IpAddress { get; private set; }
    public string? DeviceInfo { get; private set; }
    public string? CorrelationId { get; private set; }
    public DateTime OccurredAt { get; private set; }

    private AuditLog() { }

    public AuditLog(
        string entityType,
        Guid entityId,
        string action,
        string? beforeValue,
        string? afterValue,
        string performedBy,
        string? ipAddress,
        string? deviceInfo,
        string? correlationId)
    {
        if (string.IsNullOrWhiteSpace(entityType))
            throw new ArgumentException("Entity type is required.");

        if (entityId == Guid.Empty)
            throw new ArgumentException("Entity ID is required.");

        Id = Guid.NewGuid();
        EntityType = entityType;
        EntityId = entityId;
        Action = action;
        BeforeValue = beforeValue;
        AfterValue = afterValue;
        PerformedBy = performedBy;
        IpAddress = ipAddress;
        DeviceInfo = deviceInfo;
        CorrelationId = correlationId;
        OccurredAt = DateTime.UtcNow;
    }
}