public record ExternalRateResponse(
    string SourceCurrency,
    string DestinationCurrency,
    decimal BaseRate,
    DateTime FetchedAt,
    string ProviderName);

public interface IRateProviderAdapter
{
    // FetchRates: get current market reference rate from approved external provider
    // This rate feeds into the rate import flow — treasury still applies spreads and approves
    Task<ExternalRateResponse> FetchRatesAsync(
        string sourceCurrency,
        string destinationCurrency,
        CancellationToken ct = default);
}