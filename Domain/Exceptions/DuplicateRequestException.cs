public class DuplicateRequestException : Exception
{
    public DuplicateRequestException(string idempotencyKey)
        : base($"Request with idempotency key '{idempotencyKey}' has already been processed.") { }
}