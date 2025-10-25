namespace IPCountryBlocker.Domain.Models
{
    public class TemporalBlock
    {
        public Country Country { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
