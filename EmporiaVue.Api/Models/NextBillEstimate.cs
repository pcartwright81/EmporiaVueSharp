namespace EmporiaVue.Api.Models
{
    public class NextBillEstimate
    {
        public double Usage { get; set; }
        public double EstimatedUsage { get; set; }
        public double UsagePerDay { get; set; }
        public double EstimatedCost { get; set; }
        public double UsageCost { get; set; }
    }
}