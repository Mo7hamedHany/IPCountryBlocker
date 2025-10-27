using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Service.Abstractions
{
    public interface IStudentService
    {
        Task<bool> AddStudentAsync(int Id, string firstName, string lastName, int Age);
        Task<PagedResult<Student>> GetAllStudentsAsync(string? searchItem = null, int pageNumber = 1, int pageSize = 10);
        Task<bool> UpdateStudentAsync(int id, string firstName, string lastName, int age);
        Task<bool> DeleteStudentAsync(int studentId);
        Task<StudentReport> GetStudentReportByStudentId(int studentId);
    }
}
