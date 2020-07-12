namespace EmporiaVue.Api.Models
{
    public class NextBillEstimate
    {
        public bool IsEstimate { get; set; }
        public double UsageSinceDate { get; set; }
        public double EstimatedUsage { get; set; }
        public double UsagePerDay { get; set; }
        public double EstimatedCost { get; set; }
    }
}