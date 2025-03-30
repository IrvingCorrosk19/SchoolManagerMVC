using Microsoft.AspNetCore.Mvc;
using SchoolManager.Services;
using SchoolManager.Dtos;
using SchoolManager.Services.Interfaces;

public class StudentReportController : Controller
{
    private readonly IStudentReportService _reportService;

    public StudentReportController(IStudentReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<IActionResult> Index(Guid studentId, string trimester = "1T")
    {
        var report = new StudentReportDto
        {
            StudentName = "Estudiante de Prueba",
            Grade = "5to",

            Grades = new List<GradeDto>
        {
            new GradeDto
            {
                Subject = "Matemáticas",
                Teacher = "Profa. García",
                ActivityName = "Examen Parcial",
                Type = "Examen",
                Value = 8.7m,
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new GradeDto
            {
                Subject = "Ciencias",
                Teacher = "Prof. Pérez",
                ActivityName = "Proyecto",
                Type = "Tarea",
                Value = 9.3m,
                CreatedAt = DateTime.Now.AddDays(-20)
            },
            new GradeDto
            {
                Subject = "Lengua",
                Teacher = "Profa. Ramírez",
                ActivityName = "Ensayo",
                Type = "Tarea",
                Value = 7.9m,
                CreatedAt = DateTime.Now.AddDays(-5)
            }
        },

            Attendance = new List<AttendanceDto>
        {
            new AttendanceDto
            {
                Month = "Enero",
                Present = 18,
                Absent = 1,
                Late = 2
            },
            new AttendanceDto
            {
                Month = "Febrero",
                Present = 17,
                Absent = 2,
                Late = 1
            },
            new AttendanceDto
            {
                Month = "Marzo",
                Present = 20,
                Absent = 0,
                Late = 0
            }
        }
        };

        ViewBag.Trimester = trimester;
        ViewBag.StudentId = studentId;

        return View(report);
    }

}
