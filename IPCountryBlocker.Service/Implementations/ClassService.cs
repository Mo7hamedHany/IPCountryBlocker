using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Service.Abstractions;

namespace IPCountryBlocker.Service.Implementations
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<bool> AddClassAsync(int id, string name, string teacher, string description)
        {
            var @class = new Class
            {
                Id = id,
                Name = name,
                Teacher = teacher,
                Description = description
            };

            if (@class is null)
                return false;

            return await _classRepository.AddClassAsync(@class);
        }

        public async Task<PagedResult<Class>> GetAllClassAsync(string? searchItem = null, int pageNumber = 1, int pageSize = 10)
        {
            return await _classRepository.GetClassesAsync(searchItem, pageNumber, pageSize);
        }

        public async Task<Class?> GetClassByIdAsync(int classId)
        {
            return await _classRepository.GetClassById(classId);
        }

        public async Task<bool> DeleteClassAsync(int classId)
        {
            return await _classRepository.DeleteClassAsync(classId);
        }
    }
}
