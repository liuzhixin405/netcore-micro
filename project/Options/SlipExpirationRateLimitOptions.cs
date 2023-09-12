namespace project.Options
{
    public class SlipExpirationRateLimitOptions
    {
        public const string SlipExpirationRateLimit = "SlipExpirationRateLimit";
        public int PermitLimit { get; set; } = 100;
        public int Window { get; set; } = 10;
        public int ReplenishmentPeriod { get; set; } = 2;
        public int QueueLimit { get; set; } = 2;
        public int SegmentsPerWindow { get; set; } = 8;
        public int TokenLimit { get; set; } = 10;
        public int TokenLimit2 { get; set; } = 20;
        public int TokensPerPeriod { get; set; } = 4;
        public bool AutoReplenishment { get; set; } = false;
    }
}
