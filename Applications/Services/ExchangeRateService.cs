public class ExtensionService
{
    private readonly ICorridorRepository _corridorRepository;
    private readonly ICorridorRateRepository _corridorRateRepository;
    private readonly IRateApprovalLogRepository _rateApprovalRepository;

    private readonly IAuditLogRepository _auditService;
    private const decimal VarianceThresholdPercent = 5.0m; // configurable


    public ExtensionService(IAuditLogRepository auditlogRepository, ICorridorRepository corridorRepository, ICorridorRateRepository corridorRateRepository, IRateApprovalLogRepository rateApprovalLogRepository)


    {
        _corridorRepository = corridorRepository;
        _corridorRateRepository = corridorRateRepository;
        _rateApprovalRepository = rateApprovalLogRepository;
        _auditService = auditlogRepository;


    }
public async Task<Guid> SubmitRateSheetAsync(
    string corridorCode,
    string request, 
    decimal baseRate,
    decimal treasurySpread,
    decimal partnerSpread,
    decimal promotionalAdjustment,
    DateTime effectiveFrom,
    string submittedBy,
    string sourceIp,
    CancellationToken  ct)
{
    // Step 1: Fetch corridor
    var corridor = await _corridorRepository.GetByCodeAsync(corridorCode, ct);
        if (corridor == null)
        throw new CorridorInactiveException(corridorCode);

         if (baseRate <= 0)
            throw new ArgumentException("Base rate must be positive.", nameof(baseRate));


              var customerRate = baseRate - treasurySpread - partnerSpread + promotionalAdjustment;
        if (customerRate <= 0)
            throw new NegativeSpreadException(
                $"Computed customer rate {customerRate} is not positive. Check spread configuration.");

        // Step 4: Fetch current active rate. Check variance against threshold.
        var currentActive = await _corridorRateRepository
            .GetCurrentActiveRateAsync(corridor.Id, DateTime.UtcNow, ct);

        if (currentActive != null)
        {
            var variance = Math.Abs(
                (customerRate - currentActive.CustomerRate) / currentActive.CustomerRate) * 100;

            if (variance > VarianceThresholdPercent)
                throw new RateVarianceThresholdException(variance, VarianceThresholdPercent);
        }

    
}
}