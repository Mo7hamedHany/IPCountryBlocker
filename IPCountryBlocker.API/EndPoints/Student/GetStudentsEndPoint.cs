using FastEndpoints;
using IPCountryBlocker.Application.DTOs.Student;
using IPCountryBlocker.Application.Validators.Students;
using IPCountryBlocker.Service.Abstractions;

namespace IPCountryBlocker.API.EndPoints.Student
{
    public class GetStudentsEndPoint : Endpoint<GetStudentsRequest, StudentPagedResponse>
    {
        private readonly IStudentService _studentService;

        public GetStudentsEndPoint(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public override void Configure()
        {
            Get("/students");
            AllowAnonymous();
            Validator<GetStudentsValidator>();
            Options(x => x.WithTags("Student"));
        }

        public override async Task HandleAsync(GetStudentsRequest req, CancellationToken ct)
        {
            var result = await _studentService.GetAllStudentsAsync(req.SearchItem, req.PageNumber, req.PageSize);

            if (result == null || result.Items == null || !result.Items.Any())
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var response = new StudentPagedResponse
            {
                Items = result.Items,
                TotalCount = result.TotalCount,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize,
                TotalPages = (int)Math.Ceiling(result.TotalCount / (double)req.PageSize)
            };

            await SendOkAsync(response, ct);
        }
    }
}