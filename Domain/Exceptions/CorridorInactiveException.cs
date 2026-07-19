public class CorridorInactiveException : Exception
{
    public CorridorInactiveException(string corridorCode)
        : base($"Corridor '{corridorCode}' is not active and cannot accept quotes or transactions.") { }
}