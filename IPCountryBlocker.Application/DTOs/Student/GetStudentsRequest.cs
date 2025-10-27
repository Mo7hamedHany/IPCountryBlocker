namespace IPCountryBlocker.Application.DTOs.Student
{
    public class GetStudentsRequest
    {
        public string? SearchItem { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
