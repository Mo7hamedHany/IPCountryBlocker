namespace IPCountryBlocker.Domain.Models
{
    public class BlockedAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string IpAddress { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string BlockedStatus { get; set; }
        public bool IsBlocked { get; set; }
        public string UserAgent { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
