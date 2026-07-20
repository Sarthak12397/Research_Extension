public class RemittanceTransaction
{
    
public Guid Id { get; private set; }
    public string ReferenceNumber { get; private set; }        // TXN-{YYYYMMDD}-{seq}
    public Guid QuoteId { get; private set; }
    public Guid CorridorId { get; private set; }
    public Guid SenderId { get; private set; }
    public Guid BeneficiaryId { get; private set; }
    public PayoutMethod PayoutMethod { get; private set; }

    // IMMUTABLE financial snapshot — copied from Quote at creation, never updated
    public decimal LockedCustomerRate { get; private set; }
    public decimal LockedSettlementRate { get; private set; }
    public decimal SendAmount { get; private set; }
    public string SendCurrency { get; private set; }
    public decimal ServiceFee { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal TotalPayable { get; private set; }
    public decimal PayoutAmount { get; private set; }
    public string PayoutCurrency { get; private set; }

    // State machine
    public TransactionStatus Status { get; private set; }
    public string? ComplianceHoldReason { get; private set; }
    public string? FailureReason { get; private set; }
    public string? CancellationReason { get; private set; }
    public string? RefundReason { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? SubmittedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string CreatedBy { get; private set; }
    public uint RowVersion { get; private set; }

    // Navigation
    private readonly List<PayoutInstruction> _payoutInstructions = new();
    public IReadOnlyCollection<PayoutInstruction> PayoutInstructions => _payoutInstructions.AsReadOnly();

    private RemittanceTransaction() { }


     public RemittanceTransaction(
        Quote quote,
        decimal lockedSettlementRate,
        string referenceNumber,
        string sendCurrency,
        string payoutCurrency,
        string createdBy)
    {
        if (quote.IsExpired)
            throw new RateExpiredException(quote.QuoteReference);
        if (quote.IsConverted)
            throw new QuoteAlreadyConvertedException(quote.QuoteReference);
        if (lockedSettlementRate <= 0)
            throw new ArgumentException("Locked settlement rate must be positive.");
        if (string.IsNullOrWhiteSpace(referenceNumber))
            throw new ArgumentException("Reference number is required.");

        Id = Guid.NewGuid();
        ReferenceNumber = referenceNumber;
        QuoteId = quote.Id;
        CorridorId = quote.CorridorId;
        SenderId = quote.SenderId;
        BeneficiaryId = quote.BeneficiaryId;
        PayoutMethod = quote.PayoutMethod;

        // Financial snapshot
        LockedCustomerRate = quote.LockedCustomerRate;
        LockedSettlementRate = lockedSettlementRate;
        SendAmount = quote.SendAmount;
        SendCurrency = sendCurrency;
        ServiceFee = quote.ServiceFee;
        TaxAmount = quote.TaxAmount;
        TotalPayable = quote.TotalPayable;
        PayoutAmount = quote.PayoutAmount;
        PayoutCurrency = payoutCurrency;

        Status = TransactionStatus.Submitted;
        CreatedAt = DateTime.UtcNow;
        SubmittedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }



 public void PlaceOnComplianceHold(string reason)
    {
        if (Status != TransactionStatus.Submitted)
            throw new InvalidOperationException(
                $"Cannot place a {Status} transaction on compliance hold. Expected: Submitted.");
        Status = TransactionStatus.ComplianceHold;
        ComplianceHoldReason = reason;
    }


       public void Approve()
    {
        if (Status != TransactionStatus.Submitted && Status != TransactionStatus.ComplianceHold)
            throw new InvalidOperationException(
                $"Cannot approve transaction in status {Status}. Expected: Submitted or ComplianceHold.");
        Status = TransactionStatus.Approved;
        ComplianceHoldReason = null;
    }


     public void MarkReadyForPayout()
    {
        if (Status != TransactionStatus.SentToPartner)
            throw new InvalidOperationException(
                $"Transaction must be SentToPartner before marking ready. Current: {Status}.");
        Status = TransactionStatus.ReadyForPayout;
    }



    public void MarkPaid()
    {
        if (Status != TransactionStatus.SentToPartner && Status != TransactionStatus.ReadyForPayout)
            throw new InvalidOperationException(
                $"Cannot mark transaction as Paid from status {Status}.");
        Status = TransactionStatus.Paid;
        CompletedAt = DateTime.UtcNow;
    }


    public void MarkFailed(string reason)
    {
        if (Status != TransactionStatus.Approved &&
            Status != TransactionStatus.SentToPartner &&
            Status != TransactionStatus.ReadyForPayout)
            throw new InvalidOperationException(
                $"Cannot mark transaction as Failed from status {Status}.");
        Status = TransactionStatus.Failed;
        FailureReason = reason;
    }


     public void Cancel(string reason)
    {
        var nonCancellableStatuses = new[]
        {
            TransactionStatus.Paid,
            TransactionStatus.Refunded,
            TransactionStatus.Reversed,
            TransactionStatus.SentToPartner,
            TransactionStatus.ReadyForPayout
        };

        if (nonCancellableStatuses.Contains(Status))
            throw new InvalidOperationException(
                $"Cannot cancel a {Status} transaction. Use Reverse for post-partner transactions.");

        Status = TransactionStatus.Cancelled;
        CancellationReason = reason;
    }

    // Refund
    // Guards: Status must be Cancelled or Failed
    // Effects: Status = Refunded, RefundReason set, CompletedAt set
    public void Refund(string reason)
    {
        if (Status != TransactionStatus.Cancelled && Status != TransactionStatus.Failed)
            throw new InvalidOperationException(
                $"Can only refund Cancelled or Failed transactions. Current: {Status}.");
        Status = TransactionStatus.Refunded;
        RefundReason = reason;
        CompletedAt = DateTime.UtcNow;
    }

    // Reverse
    // Guards: Status must be Paid or SentToPartner
    // Effects: Status = Reversed, CompletedAt set
    public void Reverse()
    {
        if (Status != TransactionStatus.Paid && Status != TransactionStatus.SentToPartner)
            throw new InvalidOperationException(
                $"Can only reverse Paid or SentToPartner transactions. Current: {Status}.");
        Status = TransactionStatus.Reversed;
        CompletedAt = DateTime.UtcNow;
    }



    

}