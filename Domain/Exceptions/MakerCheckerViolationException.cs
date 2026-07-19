public class MakerCheckerViolationException : Exception
{
    public MakerCheckerViolationException()
        : base("The maker and checker must be different users. Same-user approval is not permitted.") { }
}