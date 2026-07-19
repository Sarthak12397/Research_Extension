public class ComplianceHoldException : Exception
{
    public ComplianceHoldException(string referenceNumber, string reason)
        : base($"Transaction '{referenceNumber}' placed on compliance hold. Reason: {reason}") { }
}