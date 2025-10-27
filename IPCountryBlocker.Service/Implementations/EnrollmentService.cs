using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Service.Abstractions;

namespace IPCountryBlocker.Service.Implementations
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRepository _classRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, IStudentRepository studentRepository, IClassRepository classRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
            _classRepository = classRepository;
        }

        public async Task<bool> EnrollStudentAsync(int enrollmentId, int studentId, int classId)
        {
            var student = await _studentRepository.GetStudentById(studentId);
            var @class = await _classRepository.GetClassById(classId);

            if (student == null || @class == null)
                return false;

            var enrollment = new Enrollment
            {
                Id = enrollmentId,
                StudentId = studentId,
                ClassId = classId,
                EnrollmentDate = DateTime.UtcNow
            };

            return await _enrollmentRepository.EnrollStudentInClassAsync(enrollment);
        }

    }
}
