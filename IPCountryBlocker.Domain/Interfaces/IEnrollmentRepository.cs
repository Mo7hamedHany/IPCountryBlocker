using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Domain.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<bool> EnrollStudentInClassAsync(Enrollment enrollment);
        Task<bool> IsAlreadyEnrolled(int studentId, int classId);
        Task<Enrollment> GetEnrollmentByIdAsync(int id);
        Task<List<int>> GetClassesByStudentIdAsync(int studentId);
    }
}
