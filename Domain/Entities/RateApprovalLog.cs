public class RateApprovalLog
{
      public Guid Id { get; private set; }
    public Guid CorridorRateId { get; private set; }
    public string CorridorCode { get; private set; }
    public string Action { get; private set; }             // SUBMITTED, APPROVED, REJECTED, EXPIRED
    public decimal OldCustomerRate { get; private set; }   // Rate before this change (0 if first submission)
    public decimal NewCustomerRate { get; private set; }
    public decimal VariancePercent { get; private set; }   // Abs((New-Old)/Old)*100
    public string? Reason { get; private set; }
    public string PerformedBy { get; private set; }
    public string? IpAddress { get; private set; }
    public DateTime OccurredAt { get; private set; }


        private RateApprovalLog() { }



            public RateApprovalLog(
        Guid corridorRateId,
        string corridorCode,
        string action,
        decimal oldCustomerRate,
        decimal newCustomerRate,
        string? reason,
        string performedBy,
        string? ipAddress)
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action is required.");
        if (string.IsNullOrWhiteSpace(performedBy))
            throw new ArgumentException("PerformedBy is required.");

        var variancePercent = oldCustomerRate > 0
            ? Math.Abs((newCustomerRate - oldCustomerRate) / oldCustomerRate) * 100
            : 0;

        Id = Guid.NewGuid();
        CorridorRateId = corridorRateId;
        CorridorCode = corridorCode;
        Action = action;
        OldCustomerRate = oldCustomerRate;
        NewCustomerRate = newCustomerRate;
        VariancePercent = variancePercent;
        Reason = reason;
        PerformedBy = performedBy;
        IpAddress = ipAddress;
        OccurredAt = DateTime.UtcNow;
    }


}