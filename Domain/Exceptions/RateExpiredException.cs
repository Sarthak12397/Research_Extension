public class RateExpiredException : Exception
{
    public RateExpiredException(string quoteReference)
        : base($"Quote '{quoteReference}' rate lock has expired. Generate a new quote.") { }
}