public class SettlementBatch
{
    public Guid Id { get; private set; }
    public string BatchReference { get; private set; }
    public string PartnerCode { get; private set; }
    public string CorridorCode { get; private set; }
    public string PayoutCurrency { get; private set; }
    public decimal TotalPayoutAmount { get; private set; }
    public decimal TotalSettlementAmount { get; private set; }
    public decimal TotalPartnerCommission { get; private set; }
    public decimal TotalFxMarginIncome { get; private set; }
    public int TransactionCount { get; private set; }
    public SettlementBatchStatus Status { get; private set; }
    public string? MismatchDetails { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public DateTime SettlementDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; }

    private SettlementBatch() { }

    public SettlementBatch(
        string batchReference,
        string partnerCode,
        string corridorCode,
        string payoutCurrency,
        decimal totalPayoutAmount,
        decimal totalSettlementAmount,
        decimal totalPartnerCommission,
        decimal totalFxMarginIncome,
        int transactionCount,
        DateTime settlementDate,
        string createdBy)
    {
        if (transactionCount <= 0)
            throw new ArgumentException("Transaction count must be positive.");

        if (totalPayoutAmount < 0)
            throw new ArgumentException("Total payout amount cannot be negative.");

        Id = Guid.NewGuid();
        BatchReference = batchReference;
        PartnerCode = partnerCode;
        CorridorCode = corridorCode;
        PayoutCurrency = payoutCurrency;
        TotalPayoutAmount = totalPayoutAmount;
        TotalSettlementAmount = totalSettlementAmount;
        TotalPartnerCommission = totalPartnerCommission;
        TotalFxMarginIncome = totalFxMarginIncome;
        TransactionCount = transactionCount;
        Status = SettlementBatchStatus.Pending;
        SettlementDate = settlementDate;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void FlagMismatch(string details)
    {
        Status = SettlementBatchStatus.Mismatched;
        MismatchDetails = details;
    }

    public void Approve(string approvedBy)
    {
        if (Status == SettlementBatchStatus.Mismatched)
            throw new InvalidOperationException(
                "Cannot approve a batch with unresolved mismatches.");

        if (Status != SettlementBatchStatus.Pending &&
            Status != SettlementBatchStatus.UnderReview)
        {
            throw new InvalidOperationException(
                $"Cannot approve batch in status {Status}.");
        }

        Status = SettlementBatchStatus.Approved;
        ApprovedBy = approvedBy;
        ApprovedAt = DateTime.UtcNow;
    }

    public void MarkSettled()
    {
        if (Status != SettlementBatchStatus.Approved)
            throw new InvalidOperationException(
                "Batch must be Approved before marking as Settled.");

        Status = SettlementBatchStatus.Settled;
    }
}