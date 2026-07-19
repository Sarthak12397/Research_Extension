public class SettlementMismatchException : Exception
{
    public SettlementMismatchException(string batchReference, string details)
        : base($"Settlement batch '{batchReference}' has reconciliation mismatches: {details}") { }
}