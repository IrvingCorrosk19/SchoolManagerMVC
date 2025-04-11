using Microsoft.EntityFrameworkCore;
using SchoolManager.Models;
using SchoolManager.Services.Interfaces;
using SchoolManager.Dtos;


namespace SchoolManager.Services.Implementations
{
    public class StudentReportService : IStudentReportService
    {
        private readonly SchoolDbContext _context;

        public StudentReportService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<StudentReportDto> GetStudentReportAsync(Guid studentId, string trimester)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                throw new Exception("Estudiante no encontrado");


            // Obtener asistencias del trimestre
            var trimesterMonths = GetMonthsByTrimester(trimester);
            var attendanceRecords = await _context.Attendances
                .Where(a => a.StudentId == studentId && trimesterMonths.Contains(a.Date.Month))
                .ToListAsync();

            var attendanceGrouped = attendanceRecords
                .GroupBy(a => a.Date.ToString("MMMM"))
                .Select(g => new AttendanceDto
                {
                    Month = g.Key,
                    Present = g.Count(x => x.Status == "present"),
                    Absent = g.Count(x => x.Status == "absent"),
                    Late = g.Count(x => x.Status == "late")
                })
                .ToList();

            return new StudentReportDto
            {
                StudentName = student.Name,
                Grade = student.Grade ?? "N/A",
                Attendance = attendanceGrouped
            };
        }

        private List<int> GetMonthsByTrimester(string trimester)
        {
            return trimester switch
            {
                "1T" => new List<int> { 1, 2, 3 },
                "2T" => new List<int> { 4, 5, 6 },
                "3T" => new List<int> { 7, 8, 9 },
                _ => new List<int>()
            };
        }
    }
}
