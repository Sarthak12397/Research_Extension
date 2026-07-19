public class QuoteAlreadyConvertedException : Exception
{
    public QuoteAlreadyConvertedException(string quoteReference)
        : base($"Quote '{quoteReference}' has already been converted to a transaction.") { }
}