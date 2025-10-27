namespace IPCountryBlocker.Application.DTOs
{
    public class BaseGetRequest
    {
        public string? SearchItem { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
