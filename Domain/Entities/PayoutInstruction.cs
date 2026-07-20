public class PayoutInstruction
{
    public Guid Id { get; private set; }
    public Guid TransactionId { get; private set; }
    public string InstructionReference { get; private set; }   // PI-{YYYYMMDD}-{seq}
    public string PartnerCode { get; private set; }
    public PayoutMethod PayoutMethod { get; private set; }
    public decimal PayoutAmount { get; private set; }
    public string PayoutCurrency { get; private set; }
    public PartnerPayoutStatus PartnerStatus { get; private set; }
    public string? PartnerRawResponse { get; private set; }    // Full JSON from partner — never parsed into PII logs
    public string? PartnerTransactionReference { get; private set; } // Partner's own reference number
    public string? FailureReason { get; private set; }
    public int AttemptCount { get; private set; }
    public string IdempotencyKey { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastAttemptAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private PayoutInstruction() { }

    // Guards: payoutAmount > 0, partnerCode not empty, idempotencyKey not empty
    // Effects: PartnerStatus = Pending, AttemptCount = 0
    public PayoutInstruction(
        Guid transactionId,
        string instructionReference,
        string partnerCode,
        PayoutMethod payoutMethod,
        decimal payoutAmount,
        string payoutCurrency,
        string idempotencyKey)
    {
        if (transactionId == Guid.Empty)
            throw new ArgumentException("Transaction ID is required.");
        if (payoutAmount <= 0)
            throw new ArgumentException("Payout amount must be positive.");
        if (string.IsNullOrWhiteSpace(partnerCode))
            throw new ArgumentException("Partner code is required.");
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new ArgumentException("Idempotency key is required.");

        Id = Guid.NewGuid();
        TransactionId = transactionId;
        InstructionReference = instructionReference;
        PartnerCode = partnerCode;
        PayoutMethod = payoutMethod;
        PayoutAmount = payoutAmount;
        PayoutCurrency = payoutCurrency;
        IdempotencyKey = idempotencyKey;
        PartnerStatus = PartnerPayoutStatus.Pending;
        AttemptCount = 0;
        CreatedAt = DateTime.UtcNow;
    }

    // Domain method: RecordAttempt
    // Effects: AttemptCount++, LastAttemptAt set, raw response stored
    public void RecordAttempt(string rawResponse, string? partnerReference = null)
    {
        AttemptCount++;
        LastAttemptAt = DateTime.UtcNow;
        PartnerRawResponse = rawResponse;
        if (partnerReference != null)
            PartnerTransactionReference = partnerReference;
    }

    // State transitions — called after partner response is received
    public void MarkAccepted() => PartnerStatus = PartnerPayoutStatus.Accepted;

    public void MarkReady() => PartnerStatus = PartnerPayoutStatus.Ready;

    public void MarkPaid()
    {
        PartnerStatus = PartnerPayoutStatus.Paid;
        CompletedAt = DateTime.UtcNow;
    }

    public void MarkRejected(string reason)
    {
        PartnerStatus = PartnerPayoutStatus.Rejected;
        FailureReason = reason;
        CompletedAt = DateTime.UtcNow;
    }

    public void MarkReversed()
    {
        PartnerStatus = PartnerPayoutStatus.Reversed;
        CompletedAt = DateTime.UtcNow;
    }
}