using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManager.Dtos;
using SchoolManager.Interfaces;
using SchoolManager.Models;
using SchoolManager.Services;
using SchoolManager.ViewModels;

namespace SchoolManager.Controllers
{
  //  [Authorize(Roles = "Teacher")] // Opcional: restringir solo a docentes
    public class TeacherGradebookController : Controller
    {
        private readonly ITrimesterService _trimesterSvc;
        private readonly ITeacherGroupService _groupSvc;
        private readonly IActivityTypeService _typeSvc;
        private readonly IActivityService _activitySvc;
        private readonly IStudentActivityScoreService _scoreSvc;
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;


        public TeacherGradebookController(
            ITrimesterService trimesterSvc,
            ITeacherGroupService groupSvc,
            IActivityTypeService typeSvc,
            IActivityService activitySvc,
            IStudentActivityScoreService scoreSvc,
            IUserService userService,
            IStudentService studentService)
            
        {
            _studentService = studentService;   
            _trimesterSvc = trimesterSvc;
            _groupSvc = groupSvc;
            _typeSvc = typeSvc;
            _activitySvc = activitySvc;
            _scoreSvc = scoreSvc;
            _userService = userService;
            
        }
        [HttpGet]
        public async Task<JsonResult> StudentsByGroupAndGrade(Guid groupId, Guid gradeId)
        {
            var students = await _studentService.GetByGroupAndGradeAsync(groupId, gradeId);
            return Json(students);
        }
        private Guid GetTeacherId()
        {
            // Supone que el claim del usuario contiene el Id como GUID
            //var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;     
            
            var userId =("1bbb72ba-396c-4b96-bd30-8a7a3997c2fc");
            if (Guid.TryParse(userId, out var id)) return id;

            throw new UnauthorizedAccessException("No se pudo obtener el ID del docente.");
        }
        public async Task<IActionResult> Index()
        {
            var teacherId = GetTeacherId();

            // 🔹 Obtener todos los docentes con asignaciones y filtrar solo al actual
            var teachers = await _userService.GetAllWithAssignmentsByRoleAsync("teacher");
            var teacher = teachers.FirstOrDefault(t => t.Id == teacherId);

            if (teacher == null)
                return NotFound("No se encontró el docente actual.");

            // 🔹 Preparar DTO visual con materias y grupos
            var assignments = teacher.TeacherAssignments ?? new List<TeacherAssignment>();

            var subjectGroupDetails = assignments.Any()
               ? assignments
                   .GroupBy(a => new
                   {
                       SubjectId = a.SubjectAssignment.SubjectId,
                       SubjectName = a.SubjectAssignment.Subject?.Name ?? "(Sin materia)"
                   })
                   .Select(g => new SubjectGroupSummary
                   {
                       SubjectId = g.Key.SubjectId,
                       SubjectName = g.Key.SubjectName,
                       GroupGradePairs = g.Select(x => new GroupGradeItem
                       {
                           GroupId = x.SubjectAssignment.GroupId,
                           GradeLevelId = x.SubjectAssignment.GradeLevelId,
                           GroupName = x.SubjectAssignment.Group?.Name ?? "(Grupo)",
                           GradeLevelName = x.SubjectAssignment.GradeLevel?.Name ?? "(Grado)"
                       })
                       .Distinct()
                       .ToList()
                   }).ToList()
               : new List<SubjectGroupSummary>();


            var teacherInfo = new TeacherAssignmentDisplayDto
            {
                TeacherId = teacher.Id,
                FullName = teacher.Name,
                Email = teacher.Email,
                Role = teacher.Role,
                IsActive = string.Equals(teacher.Status, "active", StringComparison.OrdinalIgnoreCase),
                SubjectGroupDetails = subjectGroupDetails
            };

            // 🔹 Obtener catálogo para filtros
            var trimesters = (await _trimesterSvc.GetAllAsync()).ToList();
            var firstTrim = trimesters.FirstOrDefault()?.Name ?? "";
            var groups = await _groupSvc.GetByTeacherAsync(teacherId, firstTrim);
            var types = await _typeSvc.GetAllAsync();

            // 🔹 ViewModel que combinamos con datos del docente y catálogos
            var viewModel = new TeacherGradebookViewModel
            {
                Teacher = teacherInfo,
                Trimesters = trimesters,
                Groups = groups,
                Types = types,
                TeacherId = teacherId
            };

            return View(viewModel);
        }


        // GET: /TeacherGradebook/GradeBookJson?groupId=...&trimester=...
        [HttpGet]
        public async Task<JsonResult> GradeBookJson(Guid groupId, string trimester)
        {
            var teacherId = GetTeacherId();
            var book = await _scoreSvc.GetGradeBookAsync(teacherId, groupId, trimester);
            return Json(book);
        }

        // POST: /TeacherGradebook/CreateActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateActivity([FromForm] ActivityCreateDto dto)
        {
            try
            {
                var result = await _activitySvc.CreateAsync(dto);
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // POST: /TeacherGradebook/SaveScores
        [HttpPost]
        public async Task<IActionResult> SaveScores([FromBody] StudentActivityScoreCreateDto[] scores)
        {
            try
            {
                await _scoreSvc.SaveAsync(scores);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        // DELETE: /TeacherGradebook/DeleteActivity/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            try
            {
                await _activitySvc.DeleteAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}
