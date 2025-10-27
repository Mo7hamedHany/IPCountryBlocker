namespace IPCountryBlocker.Application.DTOs.Class
{
    public class ClassesPagedResponse : BasePagedResponse
    {
        public List<Domain.Models.Class> Classes { get; set; } = new();
    }
}
