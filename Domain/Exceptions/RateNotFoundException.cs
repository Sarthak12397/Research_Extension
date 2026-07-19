public class RateNotFoundException : Exception
{
    public RateNotFoundException(string corridorCode)
        : base($"No active approved exchange rate found for corridor '{corridorCode}'.") { }
}