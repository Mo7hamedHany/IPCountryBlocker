namespace IPCountryBlocker.Application.DTOs.Enrollment
{
    public class EnrollStudentInClassRequest
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
    }
}
