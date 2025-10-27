using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Domain.Interfaces
{
    public interface IStudentRepository
    {
        Task<bool> AddStudentAsync(Student student);
        Task<Student?> GetStudentById(int studentId);
        Task<PagedResult<Student>> GetStudentsAsync(string? searchItem = null, int pageNumber = 1, int pageSize = 10);
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int studentId);
    }
}
