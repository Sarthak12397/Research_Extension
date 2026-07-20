public class Corridor
{
     public Guid Id { get; private set; }
    public string CorridorCode { get; private set; }            // UAE_AED_TO_NPL_NPR
    public string SourceCountry { get; private set; }
    public string SourceCurrency { get; private set; }
    public string DestinationCountry { get; private set; }
    public string DestinationCurrency { get; private set; }
    public List<PayoutMethod> SupportedPayoutMethods { get; private set; }
    public string DefaultPartnerCode { get; private set; }
    public decimal MinAmount { get; private set; }
    public decimal MaxAmount { get; private set; }
    public decimal CustomerDailyLimit { get; private set; }
    public decimal CustomerMonthlyLimit { get; private set; }
    public Guid FeeRuleId { get; private set; }
    public Guid? KycRuleId { get; private set; }
    public TimeOnly? CutoffTime { get; private set; }           // No new transactions after this time
    public CorridorStatus Status { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public uint RowVersion { get; private set; }   



    private readonly List<CorridorRate> _rates = new();
    public IReadOnlyCollection<CorridorRate> Rates => _rates.AsReadOnly();

    private Corridor(){}


      public Corridor(
        string corridorCode,
        string sourceCountry,
        string sourceCurrency,
        string destinationCountry,
        string destinationCurrency,
        string defaultPartnerCode,
        decimal minAmount,
        decimal maxAmount,
        decimal customerDailyLimit,
        decimal customerMonthlyLimit,
        Guid feeRuleId,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(corridorCode))
            throw new ArgumentException("Corridor code is required.");
        if (string.IsNullOrWhiteSpace(sourceCurrency))
            throw new ArgumentException("Source currency is required.");
        if (string.IsNullOrWhiteSpace(destinationCurrency))
            throw new ArgumentException("Destination currency is required.");
        if (minAmount <= 0)
            throw new ArgumentException("Min amount must be greater than zero.");
        if (maxAmount <= minAmount)
            throw new ArgumentException("Max amount must be greater than min amount.");
        if (customerDailyLimit <= 0)
            throw new ArgumentException("Customer daily limit must be positive.");
        if (customerMonthlyLimit < customerDailyLimit)
            throw new ArgumentException("Monthly limit cannot be less than daily limit.");

        Id = Guid.NewGuid();
        CorridorCode = corridorCode;
        SourceCountry = sourceCountry;
        SourceCurrency = sourceCurrency;
        DestinationCountry = destinationCountry;
        DestinationCurrency = destinationCurrency;
        DefaultPartnerCode = defaultPartnerCode;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        CustomerDailyLimit = customerDailyLimit;
        CustomerMonthlyLimit = customerMonthlyLimit;
        FeeRuleId = feeRuleId;
        Status = CorridorStatus.Inactive;
        SupportedPayoutMethods = new List<PayoutMethod>();
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }



    public void Activate(string updatedBy)
    {
        if(Status == CorridorStatus.Suspended)
        {
            throw new InvalidOperationException("Suspended corridor must be reviewed before activation.");

            
        }

            Status = CorridorStatus.Active;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }




       public void Suspend(string updatedBy)
    {
        if (Status == CorridorStatus.Inactive)
            throw new InvalidOperationException("Cannot suspend an inactive corridor.");
        Status = CorridorStatus.Suspended;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }


    public void Deactivate(string updatedBy)
    {
        Status = CorridorStatus.Inactive;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }


public void AddPayoutMethod(PayoutMethod method)
    {
        if (!SupportedPayoutMethods.Contains(method))
            SupportedPayoutMethods.Add(method);
    }

  public void ValidateIsActive()
    {
        if (Status != CorridorStatus.Active)
            throw new CorridorInactiveException(CorridorCode);
    }

      public void ValidateAmount(decimal amount)
    {
        if (amount < MinAmount || amount > MaxAmount)
            throw new LimitExceededException(
                $"Amount {amount} is outside corridor limits [{MinAmount}, {MaxAmount}] for corridor {CorridorCode}.");
    }

       public void ValidatePayoutMethodSupported(PayoutMethod method)
    {
        if (!SupportedPayoutMethods.Contains(method))
            throw new InvalidOperationException(
                $"Payout method '{method}' is not supported on corridor '{CorridorCode}'.");
    }

   public bool IsCutoffExceeded()
    {
        if (CutoffTime == null) return false;
        return TimeOnly.FromDateTime(DateTime.UtcNow) > CutoffTime.Value;
    }
}
