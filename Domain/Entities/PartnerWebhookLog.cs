public class PartnerWebhookLog
{
    public Guid Id { get; private set; }
    public string PartnerCode { get; private set; }
    public string WebhookEventType { get; private set; }
    public string RawPayload { get; private set; }
    public string? HmacSignature { get; private set; }
    public bool IsSignatureValid { get; private set; }
    public bool IsProcessed { get; private set; }
    public string? ProcessingError { get; private set; }
    public Guid? RelatedTransactionId { get; private set; }
    public DateTime ReceivedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    private PartnerWebhookLog() { }

    public PartnerWebhookLog(
        string partnerCode,
        string webhookEventType,
        string rawPayload,
        string? hmacSignature,
        bool isSignatureValid)
    {
        if (string.IsNullOrWhiteSpace(partnerCode))
            throw new ArgumentException("Partner code is required.");

        if (string.IsNullOrWhiteSpace(rawPayload))
            throw new ArgumentException("Raw payload is required.");

        Id = Guid.NewGuid();
        PartnerCode = partnerCode;
        WebhookEventType = webhookEventType;
        RawPayload = rawPayload;
        HmacSignature = hmacSignature;
        IsSignatureValid = isSignatureValid;
        IsProcessed = false;
        ReceivedAt = DateTime.UtcNow;
    }

    public void ValidateSignature()
    {
        if (!IsSignatureValid)
            throw new InvalidOperationException(
                $"Invalid HMAC signature from partner '{PartnerCode}'. Webhook rejected.");
    }

    public void MarkProcessed(Guid? transactionId)
    {
        IsProcessed = true;
        RelatedTransactionId = transactionId;
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkFailed(string error)
    {
        ProcessingError = error;
        ProcessedAt = DateTime.UtcNow;
    }
}