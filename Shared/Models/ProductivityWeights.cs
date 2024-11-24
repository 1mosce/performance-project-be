namespace PerformanceProject.Shared.Models
{
    public class ProductivityWeights
    {
        public double TaskWeight { get; set; } = 1.0;
        public double TimeWeight { get; set; } = 0.8;
        public double QualityWeight { get; set; } = 1.2;
        public double EngagementWeight { get; set; } = 0.6;
    }

}
