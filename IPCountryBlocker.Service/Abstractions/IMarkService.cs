namespace IPCountryBlocker.Service.Abstractions
{
    public interface IMarkService
    {
        Task<bool> AddStudentMarkAsync(int id, int studentId, int classId, int examMark, int assignmentMark);
        Task<decimal> GetAverageClassStudentsMarkAsync(int classId);
    }
}
