namespace LiquorCabinet.Dtos
{
    public class PatchComponentBody
    {
        public string QuantityPart { get; set; } = string.Empty;
        public double? QuantityMetric { get; set; } = null;
        public double? QuantityImperial { get; set; } = null;
    }
}