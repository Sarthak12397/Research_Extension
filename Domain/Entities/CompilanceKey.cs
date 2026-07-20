public class ComplianceCase
{
    public Guid Id { get; private set; }
    public Guid TransactionId { get; private set; }
    public string CaseReference { get; private set; }
    public string TriggerReason { get; private set; }
    public string? Evidence { get; private set; }
    public ComplianceCaseStatus Status { get; private set; }
    public string? ReviewNotes { get; private set; }
    public string? ReviewedBy { get; private set; }
    public DateTime? ReviewedAt { get; private set; }
    public DateTime OpenedAt { get; private set; }
    public string OpenedBy { get; private set; }

    private ComplianceCase() { }

    public ComplianceCase(
        Guid transactionId,
        string caseReference,
        string triggerReason,
        string? evidence,
        string openedBy)
    {
        if (transactionId == Guid.Empty)
            throw new ArgumentException("Transaction ID is required.");

        if (string.IsNullOrWhiteSpace(triggerReason))
            throw new ArgumentException("Trigger reason is required.");

        Id = Guid.NewGuid();
        TransactionId = transactionId;
        CaseReference = caseReference;
        TriggerReason = triggerReason;
        Evidence = evidence;
        Status = ComplianceCaseStatus.Open;
        OpenedAt = DateTime.UtcNow;
        OpenedBy = openedBy;
    }

    public void StartReview(string reviewedBy)
    {
        if (Status != ComplianceCaseStatus.Open)
            throw new InvalidOperationException(
                $"Can only start reviewing Open cases. Current: {Status}.");

        Status = ComplianceCaseStatus.UnderReview;
        ReviewedBy = reviewedBy;
    }

    public void Approve(string reviewedBy, string notes)
    {
        if (Status != ComplianceCaseStatus.Open &&
            Status != ComplianceCaseStatus.UnderReview)
        {
            throw new InvalidOperationException(
                $"Cannot approve case in status {Status}.");
        }

        Status = ComplianceCaseStatus.Approved;
        ReviewedBy = reviewedBy;
        ReviewNotes = notes;
        ReviewedAt = DateTime.UtcNow;
    }

    public void Reject(string reviewedBy, string notes)
    {
        if (Status != ComplianceCaseStatus.Open &&
            Status != ComplianceCaseStatus.UnderReview)
        {
            throw new InvalidOperationException(
                $"Cannot reject case in status {Status}.");
        }

        Status = ComplianceCaseStatus.Rejected;
        ReviewedBy = reviewedBy;
        ReviewNotes = notes;
        ReviewedAt = DateTime.UtcNow;
    }

    public void Escalate(string reviewedBy, string notes)
    {
        Status = ComplianceCaseStatus.Escalated;
        ReviewedBy = reviewedBy;
        ReviewNotes = notes;
        ReviewedAt = DateTime.UtcNow;
    }
}