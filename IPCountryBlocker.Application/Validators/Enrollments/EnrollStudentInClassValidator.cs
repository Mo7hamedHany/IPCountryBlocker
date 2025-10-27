using FluentValidation;
using IPCountryBlocker.Application.DTOs.Enrollment;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Domain.Interfaces;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.Application.Validators.Enrollments
{
    public class EnrollStudentInClassValidator : AbstractValidator<EnrollStudentInClassRequest>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRepository _classRepository;

        public EnrollStudentInClassValidator(
            IStringLocalizer<SharedResources> localizer,
            IEnrollmentRepository enrollmentRepository,
            IStudentRepository studentRepository,
            IClassRepository classRepository)
        {
            _localizer = localizer;
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
            _classRepository = classRepository;
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(0)
                .WithMessage("Enrollment ID must be greater than 0")
                .MustAsync(EnrollmentIdDoesNotExistAsync)
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

            RuleFor(x => x.StudentId)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(0)
                .WithMessage("Student ID must be greater than 0")
                .MustAsync(StudentExistsAsync)
                .WithMessage(_localizer[SharedResourcesKeys.NotFound]);

            RuleFor(x => x.ClassId)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(0)
                .WithMessage("Class ID must be greater than 0")
                .MustAsync(ClassExistsAsync)
                .WithMessage(_localizer[SharedResourcesKeys.NotFound]);

            RuleFor(x => x)
                .MustAsync(StudentNotAlreadyEnrolledInClassAsync)
                .WithMessage("Student is already enrolled in this class")
                .When(x => x.StudentId > 0 && x.ClassId > 0);
        }

        private async Task<bool> EnrollmentIdDoesNotExistAsync(int enrollmentId, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(enrollmentId);
            return enrollment == null;
        }

        private async Task<bool> StudentExistsAsync(int studentId, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetStudentById(studentId);
            return student != null;
        }

        private async Task<bool> ClassExistsAsync(int classId, CancellationToken cancellationToken)
        {
            var classObj = await _classRepository.GetClassById(classId);
            return classObj != null;
        }

        private async Task<bool> StudentNotAlreadyEnrolledInClassAsync(EnrollStudentInClassRequest request, CancellationToken cancellationToken)
        {
            var isEnrolled = await _enrollmentRepository.IsAlreadyEnrolled(request.StudentId, request.ClassId);
            return !isEnrolled;
        }
    }
}
