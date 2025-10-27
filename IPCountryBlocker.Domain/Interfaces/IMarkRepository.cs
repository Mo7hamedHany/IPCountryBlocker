using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Domain.Interfaces
{
    public interface IMarkRepository
    {
        Task<bool> AddMarkAsync(Mark mark);
        Task<IEnumerable<Mark>> GetMarksByClassIdAsync(int classId);
        Task<Mark?> GetMarksOfStudentForClassIdAsync(int studentId, int classId);

    }
}
