using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Domain.Interfaces
{
    public interface IClassRepository
    {
        Task<bool> AddClassAsync(Class obj);
        Task<Class?> GetClassById(int classId);
        Task<PagedResult<Class>> GetClassesAsync(string? searchItem = null, int pageNumber = 1, int pageSize = 10);
        Task<bool> DeleteClassAsync(int classId);
    }
}
