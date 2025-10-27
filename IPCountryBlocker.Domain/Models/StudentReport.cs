namespace IPCountryBlocker.Domain.Models
{
    public class StudentReport
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public List<StudentClassReport> EnrolledClasses { get; set; } = new();
        public decimal OverallAverageMarkAcrossClasses { get; set; }
        public int TotalClassesEnrolled { get; set; }
        public int TotalClassesWithMarks { get; set; }
    }

}
