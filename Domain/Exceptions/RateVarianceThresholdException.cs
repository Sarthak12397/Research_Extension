public class RateVarianceThresholdException : Exception
{
    public RateVarianceThresholdException(decimal variancePercent, decimal threshold)
        : base($"Rate variance {variancePercent:F2}% exceeds threshold {threshold:F2}%. Additional approval required.") { }
}