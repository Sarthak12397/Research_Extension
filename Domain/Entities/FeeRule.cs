public class FeeRule
{
    public Guid Id { get; private set; }
    public string RuleCode { get; private set; }
    public FeeType FeeType { get; private set; }
    public decimal? FixedAmount { get; private set; }
    public decimal? PercentageRate { get; private set; }
    public decimal? TaxRate { get; private set; }
    public decimal? DiscountAmount { get; private set; }
    public string? TierConfigurationJson { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private FeeRule() { }

    public FeeRule(string ruleCode, FeeType feeType, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(ruleCode))
            throw new ArgumentException("Rule code is required.");

        Id = Guid.NewGuid();
        RuleCode = ruleCode;
        FeeType = feeType;
        IsActive = true;
        EffectiveFrom = DateTime.UtcNow;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public decimal CalculateFee(decimal sendAmount)
    {
        return FeeType switch
        {
            FeeType.Fixed =>
                FixedAmount ?? throw new InvalidOperationException("Fixed amount not configured."),

            FeeType.Percentage =>
                sendAmount * (PercentageRate ?? throw new InvalidOperationException("Percentage rate not configured.")) / 100m,

            FeeType.TierBased =>
                CalculateTierFee(sendAmount),

            FeeType.PromotionalDiscount =>
                -(DiscountAmount ?? 0m),

            FeeType.TaxVat =>
                sendAmount * (TaxRate ?? throw new InvalidOperationException("Tax rate not configured.")) / 100m,

            _ => 0m
        };
    }

    private decimal CalculateTierFee(decimal sendAmount)
    {
        if (string.IsNullOrWhiteSpace(TierConfigurationJson))
            throw new InvalidOperationException("Tier configuration is not set.");

        throw new NotImplementedException("Implement tier deserialization from TierConfigurationJson.");
    }

    public void SetFixedFee(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Fixed fee must be positive.");

        FixedAmount = amount;
    }

    public void SetPercentageFee(decimal rate)
    {
        if (rate <= 0 || rate > 100)
            throw new ArgumentException("Percentage rate must be between 0 and 100.");

        PercentageRate = rate;
    }
}