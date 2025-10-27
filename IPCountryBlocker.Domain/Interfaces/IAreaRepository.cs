using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Domain.Interfaces
{
    public interface IAreaRepository
    {
        Task<bool> AddArea(Area area);
        Task<Area?> GetAreaByNameAsync(string areaName);
        Task<List<Area>> GetAllAreasAsync();
    }
}
