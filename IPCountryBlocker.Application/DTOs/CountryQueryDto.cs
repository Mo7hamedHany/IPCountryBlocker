namespace IPCountryBlocker.Application.DTOs
{
    public class CountryQueryDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsTemporary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
