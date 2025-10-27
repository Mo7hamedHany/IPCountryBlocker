using FastEndpoints;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Service.Abstractions;

namespace IPCountryBlocker.API.EndPoints.Student
{
    public class GetStudentReportEndPoint : EndpointWithoutRequest<StudentReport>
    {
        private readonly IStudentService _studentService;

        public GetStudentReportEndPoint(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public override void Configure()
        {
            Get("/api/students/{studentId}/report");
            AllowAnonymous();
            Options(x => x.WithTags("Student"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            if (!int.TryParse(Route<string>("studentId"), out var studentId) || studentId <= 0)
            {
                ThrowError("Invalid student ID");
            }

            var report = await _studentService.GetStudentReportByStudentId(studentId);

            if (report == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendOkAsync(report, ct);
        }
    }
}