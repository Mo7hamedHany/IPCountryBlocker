using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Service.Abstractions
{
    public interface IClassService
    {
        Task<bool> AddClassAsync(int id, string name, string teacher, string description);
        Task<PagedResult<Class>> GetAllClassAsync(string? searchItem = null, int pageNumber = 1, int pageSize = 10);
        Task<Class?> GetClassByIdAsync(int classId);
        Task<bool> DeleteClassAsync(int classId);
    }
}
