public class KycRequiredException : Exception
{
    public KycRequiredException(string senderCode)
        : base($"Sender '{senderCode}' must complete KYC verification before transacting.") { }
}