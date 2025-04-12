using Microsoft.AspNetCore.Mvc;
using SchoolManager.Application.Interfaces;
using SchoolManager.Models;
using SchoolManager.Services.Interfaces;
using SchoolManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManager.Controllers
{
    public class StudentAssignmentController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISubjectService _subjectService;
        private readonly IGroupService _groupService;
        private readonly IGradeLevelService _gradeLevelService;
        private readonly IStudentAssignmentService _studentAssignmentService;

        public StudentAssignmentController(
            IUserService userService,
            ISubjectService subjectService,
            IGroupService groupService,
            IGradeLevelService gradeLevelService,
            IStudentAssignmentService studentAssignmentService)
        {
            _userService = userService;
            _subjectService = subjectService;
            _groupService = groupService;
            _gradeLevelService = gradeLevelService;
            _studentAssignmentService = studentAssignmentService;
        }

        public IActionResult Upload()
        {
            return View();
        }
        // Mostrar listado de estudiantes
        public async Task<IActionResult> Index()
        {
            var students = await _userService.GetAllStudentsAsync();
            ViewBag.Subjects = await _subjectService.GetAllAsync();
            ViewBag.Groups = await _groupService.GetAllAsync();
            ViewBag.Grades = await _gradeLevelService.GetAllAsync();

            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Assign(Guid id)
        {
            var student = await _userService.GetByIdAsync(id);
            if (student == null || student.Role?.ToLower() != "estudiante")
                return NotFound();

            var existingAssignments = await _studentAssignmentService.GetAssignmentsByStudentIdAsync(id);

            var model = new StudentAssignmentViewModel
            {
                StudentId = student.Id,
                SelectedGrades = existingAssignments.Select(x => x.GradeId).Distinct().ToList(),
                SelectedGroups = existingAssignments.Select(x => x.GroupId).Distinct().ToList(),
                AllSubjects = await _subjectService.GetAllAsync(),
                AllGrades = (await _gradeLevelService.GetAllAsync()).ToList(),
                AllGroups = await _groupService.GetAllAsync()
            };

            return View("Assign", model);
        }


        [HttpPost]
        public async Task<IActionResult> GuardarAsignacion([FromBody] StudentAssignmentRequest request)
        {
            if (request.GroupIds == null || !request.GroupIds.Any())
            {
                return BadRequest(new { success = false, message = "Debe seleccionar al menos un grupo." });
            }
              
            var insertedGroupIds = new List<Guid>();

            foreach (var groupId in request.GroupIds)
            {
                var inserted = await _studentAssignmentService.AssignStudentAsync(
                    request.UserId,
                    request.SubjectId,
                    request.GradeId,
                    groupId
                );

                if (inserted)
                {
                    insertedGroupIds.Add(groupId);
                }
            }

            if (!insertedGroupIds.Any())
            {
                return Ok(new
                {
                    success = false,
                    message = "Estas combinaciones ya existen. No se guardaron nuevas asignaciones."
                });
            }

            var subject = await _subjectService.GetByIdAsync(request.SubjectId);
            var grade = await _gradeLevelService.GetByIdAsync(request.GradeId);
            var allGroups = await _groupService.GetAllAsync();

            var insertedGroupNames = allGroups
                .Where(g => insertedGroupIds.Contains(g.Id))
                .Select(g => g.Name)
                .ToList();

            return Ok(new
            {
                request.UserId,
                request.SubjectId,
                SubjectName = subject?.Name,
                request.GradeId,
                GradeName = grade?.Name,
                GroupIds = insertedGroupIds,
                GroupNames = insertedGroupNames,
                success = true,
                message = "Asignación guardada correctamente."
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAssignments(Guid userId, List<Guid> subjectIds, List<Guid> groupIds, List<Guid> gradeLevelIds)
        {
            var user = await _userService.GetByIdWithRelationsAsync(userId);
            if (user == null) return NotFound();

            await _userService.UpdateAsync(user, subjectIds, groupIds, gradeLevelIds);

            return Json(new { success = true, message = "Asignaciones actualizadas correctamente." });
        }
        [HttpPost]
        public async Task<IActionResult> SaveAssignments([FromBody] List<StudentAssignmentInputModel> asignaciones)
        {
            if (asignaciones == null || asignaciones.Count == 0)
                return BadRequest(new { success = false, message = "No se recibieron asignaciones." });

 

            int insertadas = 0;
            int duplicadas = 0;
            var errores = new List<string>();

            foreach (var item in asignaciones)
            {
                try
                {
                    var student = await _userService.GetByEmailAsync(item.Estudiante);
                    var grade = await _gradeLevelService.GetByNameAsync(item.Grado);
                    var group = await _groupService.GetByNameAndGradeAsync(item.Grupo);

                    if (student == null || grade == null || group == null)
                    {
                        errores.Add($"Error de datos: {item.Estudiante} - {item.Grado} - {item.Grupo}");
                        continue;
                    }

                    bool exists = await _studentAssignmentService.ExistsAsync(student.Id, grade.Id, group.Id);
                    if (exists)
                    {
                        duplicadas++;
                        continue;
                    }

                    var assignment = new StudentAssignment
                    {
                        Id = Guid.NewGuid(),
                        StudentId = student.Id,
                        GradeId = grade.Id,
                        GroupId = group.Id,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _studentAssignmentService.InsertAsync(assignment);
                    insertadas++;
                }
                catch (Exception ex)
                {
                    errores.Add($"Excepción en {item.Estudiante}: {ex.Message}");
                }
            }

            return Ok(new
            {
                success = true,
                insertadas,
                duplicadas,
                errores,
                message = "Carga masiva completada."
            });
        }

    }
}
