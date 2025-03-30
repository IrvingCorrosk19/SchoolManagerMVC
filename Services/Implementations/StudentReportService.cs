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

            // Obtener calificaciones del trimestre
            var grades = await _context.Grades
                .Include(g => g.Activity)
                    .ThenInclude(a => a.Subject)
                .Include(g => g.Activity)
                    .ThenInclude(a => a.Teacher)
                .Where(g => g.StudentId == studentId && g.Activity.Trimester == trimester)
                .Select(g => new GradeDto
                {
                    Subject = g.Activity.Subject.Name,
                    Teacher = g.Activity.Teacher.Name,
                    ActivityName = g.Activity.Name,
                    Type = g.Activity.Type,
                    Value = g.Value ?? 0,
                    CreatedAt = g.CreatedAt ?? DateTime.MinValue
                })
                .ToListAsync();

            // Obtener asistencias del trimestre
            var trimesterMonths = GetMonthsByTrimester(trimester);
            var attendanceRecords = await _context.Attendance
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
                Grades = grades,
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
