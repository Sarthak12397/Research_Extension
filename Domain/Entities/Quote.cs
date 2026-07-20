public class Quote
{
    public Guid Id { get; private set; }
    public string QuoteReference { get; private set; }         // Q-{YYYYMMDD}-{seq}
    public Guid CorridorId { get; private set; }
    public Guid CorridorRateId { get; private set; }           // FK to the exact rate snapshot used
    public Guid SenderId { get; private set; }
    public Guid BeneficiaryId { get; private set; }
    public PayoutMethod PayoutMethod { get; private set; }
    public CalculationDirection Direction { get; private set; }

    // All amounts are IMMUTABLE after creation — never recalculate from a live rate
    public decimal SendAmount { get; private set; }
    public decimal ServiceFee { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal TotalPayable { get; private set; }           // SendAmount + ServiceFee + TaxAmount
    public decimal LockedCustomerRate { get; private set; }     // Snapshot from CorridorRate.CustomerRate
    public decimal PayoutAmount { get; private set; }           // SendAmount * LockedCustomerRate

    public bool IsConverted { get; private set; }
    public DateTime RateExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; }
    public uint RowVersion { get; private set; }               // Concurrency — prevent double-convert race

    // Computed — NOT stored in DB
    public bool IsExpired => DateTime.UtcNow > RateExpiresAt && !IsConverted;

    private Quote() { }



     public Quote(
        Guid corridorId,
        Guid corridorRateId,
        Guid senderId,
        Guid beneficiaryId,
        PayoutMethod payoutMethod,
        CalculationDirection direction,
        decimal sendAmount,
        decimal serviceFee,
        decimal taxAmount,
        decimal lockedCustomerRate,
        decimal payoutAmount,
        int rateLockMinutes,
        string quoteReference,
        string createdBy)
    {
        if (sendAmount <= 0)
            throw new ArgumentException("Send amount must be positive.");
        if (lockedCustomerRate <= 0)
            throw new ArgumentException("Locked customer rate must be positive.");
        if (payoutAmount <= 0)
            throw new ArgumentException("Payout amount must be positive.");
        if (rateLockMinutes <= 0)
            throw new ArgumentException("Rate lock minutes must be positive.");
        if (string.IsNullOrWhiteSpace(quoteReference))
            throw new ArgumentException("Quote reference is required.");

        Id = Guid.NewGuid();
        QuoteReference = quoteReference;
        CorridorId = corridorId;
        CorridorRateId = corridorRateId;
        SenderId = senderId;
        BeneficiaryId = beneficiaryId;
        PayoutMethod = payoutMethod;
        Direction = direction;
        SendAmount = sendAmount;
        ServiceFee = serviceFee;
        TaxAmount = taxAmount;
        TotalPayable = sendAmount + serviceFee + taxAmount;
        LockedCustomerRate = lockedCustomerRate;
        PayoutAmount = payoutAmount;
        IsConverted = false;
        RateExpiresAt = DateTime.UtcNow.AddMinutes(rateLockMinutes);
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }



       public void ValidateNotExpired()
    {
        if (IsExpired)
            throw new RateExpiredException(QuoteReference);
    }


       public void ValidateNotConverted()
    {
        if (IsConverted)
            throw new QuoteAlreadyConvertedException(QuoteReference);
    }


        public void MarkAsConverted()
    {
        ValidateNotExpired();
        ValidateNotConverted();
        IsConverted = true;
    }

}