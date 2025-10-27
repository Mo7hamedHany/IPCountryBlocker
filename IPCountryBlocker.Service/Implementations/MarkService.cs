using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Service.Abstractions;

namespace IPCountryBlocker.Service.Implementations
{
    public class MarkService : IMarkService
    {
        private readonly IMarkRepository _markRepository;

        public MarkService(IMarkRepository markRepository)
        {
            _markRepository = markRepository;
        }

        public async Task<bool> AddStudentMarkAsync(int id, int studentId, int classId, int examMark, int assignmentMark)
        {
            var mark = new Mark
            {
                Id = id,
                StudentId = studentId,
                ClassId = classId,
                ExamMark = examMark,
                AssignmentMark = assignmentMark
            };

            if (mark is null)
                return false;

            var result = await _markRepository.AddMarkAsync(mark);

            return result;
        }

        public async Task<decimal> GetAverageClassStudentsMarkAsync(int classId)
        {
            var marks = await _markRepository.GetMarksByClassIdAsync(classId);

            if (marks == null || !marks.Any())
                return 0;

            var totalMarks = marks.Sum(m => m.ExamMark + m.AssignmentMark);
            var averageMark = totalMarks / marks.Count();

            return averageMark;

        }
    }
}
