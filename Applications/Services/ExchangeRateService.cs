public class ExtensionService
{
    private readonly ICorridorRepository _corridorRepository;
    private readonly ICorridorRateRepository _CorridorRateRepository;
    private readonly IRateApprovalLogRepository _rateApprovalRepository;



    public ExtensionService()
    {
        
    }
    public async Task<Guid> SubmitRateSheetAsync(
    RateSheetRequest request,
    string submittedBy,
    string sourceIp)
{
    // async implementation
    return rateId;
}
}