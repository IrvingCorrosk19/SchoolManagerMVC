using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SchoolManager.Models;
using SchoolManager.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManager.Controllers
{
    public class SubjectAssignmentController : Controller
    {
        private readonly SchoolDbContext _context;

        public SubjectAssignmentController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View(new List<SubjectAssignmentPreview>());
        }
        [HttpPost]
        public async Task<IActionResult> SaveAssignments([FromBody] List<SubjectAssignmentPreview> asignaciones)
        {
            if (asignaciones == null || asignaciones.Count == 0)
                return BadRequest(new { success = false, message = "No se recibieron asignaciones." });

            var asignacionesCreadas = new List<string>();

            foreach (var item in asignaciones)
            {
                // Buscar entidades existentes
                var materia = await _context.Subjects.FirstOrDefaultAsync(s => s.Name.ToLower() == item.Materia.ToLower());
                var grado = await _context.GradeLevels.FirstOrDefaultAsync(g => g.Name.ToLower() == item.Grado.ToLower());
                var grupo = await _context.Groups.FirstOrDefaultAsync(g => g.Name.ToLower() == item.Grupo.ToLower());

                if (materia != null && grado != null && grupo != null)
                {
                    // Validar si ya existe
                    bool yaExiste = await _context.SubjectAssignments.AnyAsync(a =>
                        a.SubjectId == materia.Id &&
                        //a.GradeId == grado.Id &&
                        a.GroupId == grupo.Id);

                    if (!yaExiste)
                    {
                        _context.SubjectAssignments.Add(new SubjectAssignment
                        {
                            Id = Guid.NewGuid(),
                            SubjectId = materia.Id,
                            //GradeId = grado.Id,
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

        //[HttpPost]
        //public IActionResult SaveAssignments([FromBody] List<SubjectAssignmentPreview> asignaciones)
        //{
        //    if (asignaciones == null || asignaciones.Count == 0)
        //        return BadRequest(new { success = false, message = "No se recibieron asignaciones." });

        //    // Solo mostrar los datos recibidos
        //    return Ok(new
        //    {
        //        success = true,
        //        count = asignaciones.Count,
        //        message = "Datos recibidos correctamente.",
        //        data = asignaciones
        //    });
        //}

    }
}
