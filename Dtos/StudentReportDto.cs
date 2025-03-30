namespace SchoolManager.Dtos
{
    public class StudentReportDto
    {
        public string StudentName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public List<GradeDto> Grades { get; set; } = new();
        public List<AttendanceDto> Attendance { get; set; } = new();
    }

}
