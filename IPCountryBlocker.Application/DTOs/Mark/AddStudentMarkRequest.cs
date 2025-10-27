namespace IPCountryBlocker.Application.DTOs.Mark
{
    public class AddStudentMarkRequest
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public decimal ExamMark { get; set; }
        public decimal AssignmentMark { get; set; }
    }
}
