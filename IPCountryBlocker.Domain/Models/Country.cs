using IPCountryBlocker.Domain.Interfaces;

namespace IPCountryBlocker.Domain.Models
{
    public class Country : ITimeTrackEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsTemporary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
