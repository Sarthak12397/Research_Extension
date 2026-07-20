public class IdempotencyRecord
{
    public Guid Id { get; private set; }
    public string IdempotencyKey { get; private set; }
    public string OperationType { get; private set; }
    public string? ResponseSnapshot { get; private set; }
    public int HttpStatusCode { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    private IdempotencyRecord() { }

    public IdempotencyRecord(
        string idempotencyKey,
        string operationType,
        string? responseSnapshot,
        int httpStatusCode,
        int expiryHours = 24)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new ArgumentException("Idempotency key is required.");

        if (string.IsNullOrWhiteSpace(operationType))
            throw new ArgumentException("Operation type is required.");

        if (expiryHours <= 0)
            throw new ArgumentException("Expiry hours must be positive.");

        Id = Guid.NewGuid();
        IdempotencyKey = idempotencyKey;
        OperationType = operationType;
        ResponseSnapshot = responseSnapshot;
        HttpStatusCode = httpStatusCode;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddHours(expiryHours);
    }

    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
}