namespace IPCountryBlocker.Application.DTOs.Student
{
    public class StudentPagedResponse
    {
        public List<IPCountryBlocker.Domain.Models.Student> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}