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