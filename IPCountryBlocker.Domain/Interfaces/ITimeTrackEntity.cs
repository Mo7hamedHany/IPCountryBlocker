namespace IPCountryBlocker.Domain.Interfaces
{
    public interface ITimeTrackEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
