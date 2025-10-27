using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Service.Abstractions;

namespace IPCountryBlocker.Service.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMarkRepository _markRepository;


        public StudentService(IStudentRepository studentRepository, IEnrollmentRepository enrollmentRepository, IClassRepository classRepository, IMarkRepository markRepository)
        {
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
            _classRepository = classRepository;
            _markRepository = markRepository;
        }

        public async Task<bool> AddStudentAsync(int Id, string firstName, string lastName, int Age)
        {
            var student = new Student
            {
                Id = Id,
                FirstName = firstName,
                LastName = lastName,
                Age = Age,
            };

            if (student is null)
                return false;

            return await _studentRepository.AddStudentAsync(student);
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var existingStudent = await _studentRepository.GetStudentById(studentId);

            if (existingStudent is null)
                return false;

            return await _studentRepository.DeleteStudentAsync(studentId);
        }

        public async Task<PagedResult<Student>> GetAllStudentsAsync(string? searchItem = null, int pageNumber = 1, int pageSize = 10)
        {
            return await _studentRepository.GetStudentsAsync(searchItem, pageNumber, pageSize);
        }

        public async Task<StudentReport> GetStudentReportByStudentId(int studentId)
        {
            var student = await _studentRepository.GetStudentById(studentId);
            if (student is null)
                return null;

            var enrolledClassIds = await _enrollmentRepository.GetClassesByStudentIdAsync(studentId);

            var enrolledClasses = new List<StudentClassReport>();
            decimal totalMarksSum = 0;
            int classesWithMarks = 0;

            foreach (var classId in enrolledClassIds)
            {
                var classObj = await _classRepository.GetClassById(classId);
                if (classObj == null)
                    continue;

                var studentMark = await _markRepository.GetMarksOfStudentForClassIdAsync(studentId, classId);

                var classReport = new StudentClassReport
                {
                    ClassId = classObj.Id,
                    ClassName = classObj.Name,
                    Teacher = classObj.Teacher,
                    ExamMark = studentMark?.ExamMark,
                    AssignmentMark = studentMark?.AssignmentMark,
                    TotalMark = studentMark?.TotalMark,
                    HasMarks = studentMark != null
                };

                enrolledClasses.Add(classReport);

                if (studentMark != null)
                {
                    totalMarksSum += studentMark.TotalMark;
                    classesWithMarks++;
                }
            }

            var overallAverage = classesWithMarks > 0 ? totalMarksSum / classesWithMarks : 0;

            return new StudentReport
            {
                StudentId = student.Id,
                StudentName = $"{student.FirstName} {student.LastName}",
                EnrolledClasses = enrolledClasses,
                OverallAverageMarkAcrossClasses = Math.Round(overallAverage, 2),
                TotalClassesEnrolled = enrolledClasses.Count,
                TotalClassesWithMarks = classesWithMarks
            };
        }

        public async Task<bool> UpdateStudentAsync(int id, string firstName, string lastName, int age)
        {
            var student = new Student
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
            };

            if (student is null)
                return false;

            return await _studentRepository.AddStudentAsync(student);
        }
    }
}
