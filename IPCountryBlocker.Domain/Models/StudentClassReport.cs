namespace IPCountryBlocker.Domain.Models
{
    public class StudentClassReport
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string Teacher { get; set; }
        public decimal? ExamMark { get; set; }
        public decimal? AssignmentMark { get; set; }
        public decimal? TotalMark { get; set; }
        public bool HasMarks { get; set; }
    }
}