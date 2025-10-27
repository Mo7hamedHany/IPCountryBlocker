namespace IPCountryBlocker.Service.Abstractions
{
    public interface IEnrollmentService
    {
        Task<bool> EnrollStudentAsync(int enrollmentId, int studentId, int classId);
    }
}
