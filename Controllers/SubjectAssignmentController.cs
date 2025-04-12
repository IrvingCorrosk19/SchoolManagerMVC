using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Models;
using SchoolManager.Services.Interfaces;
using SchoolManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManager.Controllers
{
    public class SubjectAssignmentController : Controller
    {
        private readonly SchoolDbContext _context;
        private readonly IUserService _userService;
        private readonly ISubjectService _subjectService;
        private readonly IGroupService _groupService;
        private readonly IGradeLevelService _gradeLevelService;
        private readonly IStudentAssignmentService _studentAssignmentService;

        public SubjectAssignmentController(
            SchoolDbContext context,
            IUserService userService,
            ISubjectService subjectService,
            IGroupService groupService,
            IGradeLevelService gradeLevelService,
            IStudentAssignmentService studentAssignmentService)
        {
            _context = context;
            _userService = userService;
            _subjectService = subjectService;
            _groupService = groupService;
            _gradeLevelService = gradeLevelService;
            _studentAssignmentService = studentAssignmentService;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View(new List<SubjectAssignmentPreview>());
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

        [HttpPost]
        public async Task<IActionResult> SaveAssignmentsSibgle([FromBody] List<SubjectAssignmentPreview> asignaciones)
        {
            if (asignaciones == null || asignaciones.Count == 0)
                return BadRequest(new { success = false, message = "No se recibieron asignaciones." });

            var asignacionesCreadas = new List<string>();

            foreach (var item in asignaciones)
            {
                var materia = await _context.Subjects.FirstOrDefaultAsync(s => s.Name.ToLower() == item.Materia.ToLower());
                var grado = await _context.GradeLevels.FirstOrDefaultAsync(g => g.Name.ToLower() == item.Grado.ToLower());
                var grupo = await _context.Groups.FirstOrDefaultAsync(g => g.Name.ToLower() == item.Grupo.ToLower());

                if (materia != null && grado != null && grupo != null)
                {
                    bool yaExiste = await _context.SubjectAssignments.AnyAsync(a =>
                        a.SubjectId == materia.Id &&
                        a.GroupId == grupo.Id);

                    if (!yaExiste)
                    {
                        _context.SubjectAssignments.Add(new SubjectAssignment
                        {
                            Id = Guid.NewGuid(),
                            SubjectId = materia.Id,
                            GroupId = grupo.Id,
                            CreatedAt = DateTime.UtcNow
                        });

                        asignacionesCreadas.Add($"{materia.Name} - {grado.Name} - {grupo.Name}");
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = $"{asignacionesCreadas.Count} asignaciones guardadas.",
                detalles = asignacionesCreadas
            });
        }
    }
}
