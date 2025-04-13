﻿using Microsoft.AspNetCore.Mvc;
using SchoolManager.Application.Interfaces;
using SchoolManager.Infrastructure.Services;
using SchoolManager.Models;
using SchoolManager.Services.Interfaces;
using SchoolManager.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

public class AcademicAssignmentController : Controller
{
    private readonly ITeacherAssignmentService _teacherAssignmentService;
    private readonly IUserService _userService;
    private readonly ISubjectService _subjectService;
    private readonly IGroupService _groupService;
    private readonly IGradeLevelService _gradeLevelService;
    private readonly IAcademicAssignmentService _academicAssignmentService;
    private readonly IAreaService _areaService;
    private readonly ISpecialtyService _specialtyService;

    public AcademicAssignmentController(
        ITeacherAssignmentService teacherAssignmentService,
        IUserService userService,
        ISubjectService subjectService,
        IGroupService groupService,
        IGradeLevelService gradeLevelService,
        IAcademicAssignmentService academicAssignmentService,
        IAreaService areaService,
        ISpecialtyService specialtyService)
    {
        _teacherAssignmentService = teacherAssignmentService;
        _userService = userService;
        _subjectService = subjectService;
        _groupService = groupService;
        _gradeLevelService = gradeLevelService;
        _academicAssignmentService = academicAssignmentService;
        _areaService = areaService;
        _specialtyService = specialtyService;
    }


    public IActionResult Upload()
    {
        return View();
    }

    // Carga masiva desde archivo Excel
    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> SaveAssignmentsFromExcel([FromBody] List<AssignmentInputModel> asignaciones)
    {
        if (asignaciones == null || !asignaciones.Any())
            return BadRequest(new { message = "No se recibió información válida." });

        var asignacionesInsertadas = 0;
        var profesoresNoEncontrados = new List<string>();

        foreach (var asignacion in asignaciones)
        {
            // Normalizar entradas
            var especialidad = string.IsNullOrWhiteSpace(asignacion.Especialidad) ? "N/A" : asignacion.Especialidad.Trim().ToUpper();
            var area = string.IsNullOrWhiteSpace(asignacion.Area) ? "N/A" : asignacion.Area.Trim().ToUpper();
            var materia = string.IsNullOrWhiteSpace(asignacion.Materia) ? "N/A" : asignacion.Materia.Trim().ToUpper();
            var grado = string.IsNullOrWhiteSpace(asignacion.Grado) ? "N/A" : asignacion.Grado.Trim().ToUpper();
            var grupo = string.IsNullOrWhiteSpace(asignacion.Grupo) ? "N/A" : asignacion.Grupo.Trim().ToUpper();
            var correoDocente = asignacion.Docente?.Trim().ToLower();

            var specialty = await _specialtyService.GetOrCreateAsync(especialidad);
            var areaEntity = await _areaService.GetOrCreateAsync(area);
            var subject = await _subjectService.GetOrCreateAsync(materia);
            var grade = await _gradeLevelService.GetOrCreateAsync(grado);
            var groupEntity = await _groupService.GetOrCreateAsync(grupo);

            // Verifica si ya existe la asignación académica
            bool yaExiste = await _academicAssignmentService.ExisteAsignacionAsync(
                specialty.Id, areaEntity.Id, subject.Id, grade.Id, groupEntity.Id
            );

            // Si no existe, se crea
            if (!yaExiste)
            {
                await _academicAssignmentService.CreateAsignacionAsync(
                    specialty.Id, areaEntity.Id, subject.Id, grade.Id, groupEntity.Id
                );
                asignacionesInsertadas++;
            }

            // Obtener el subject_assignment_id
            var subjectAssignmentId = await _academicAssignmentService.GetSubjectAssignmentIdAsync(
                specialty.Id, areaEntity.Id, subject.Id, grade.Id, groupEntity.Id
            );

            if (subjectAssignmentId != null && !string.IsNullOrEmpty(correoDocente))
            {
                // Buscar docente
                var docente = await _userService.GetByEmailAsync(correoDocente);
                if (docente != null)
                {
                    await _academicAssignmentService.AssignTeacherAsync(docente.Id, subjectAssignmentId.Value);
                }
                else
                {
                    profesoresNoEncontrados.Add(correoDocente);
                }
            }
        }

        return Ok(new
        {
            message = asignacionesInsertadas > 0
                ? $"Se insertaron {asignacionesInsertadas} nuevas asignaciones."
                : "No se insertaron nuevas asignaciones. Todas ya existían.",
            profesoresSinAsignar = profesoresNoEncontrados,
            success = true
        });
    }

    //public async Task<IActionResult> SaveAssignmentsFromExcel([FromBody] List<AssignmentInputModel> asignaciones)
    //{
    //    if (asignaciones == null || !asignaciones.Any())
    //        return BadRequest(new { message = "No se recibió información válida." });

    //    var asignacionesInsertadas = 0;

    //    foreach (var asignacion in asignaciones)
    //    {
    //        // Normalización básica (espacios y mayúsculas)
    //        // Normalización con valor por defecto si está vacío o nulo
    //        var especialidad = string.IsNullOrWhiteSpace(asignacion.Especialidad) ? "N/A" : asignacion.Especialidad.Trim().ToUpper();
    //        var area = string.IsNullOrWhiteSpace(asignacion.Area) ? "N/A" : asignacion.Area.Trim().ToUpper();
    //        var materia = string.IsNullOrWhiteSpace(asignacion.Materia) ? "N/A" : asignacion.Materia.Trim().ToUpper();
    //        var grado = string.IsNullOrWhiteSpace(asignacion.Grado) ? "N/A" : asignacion.Grado.Trim().ToUpper();
    //        var grupo = string.IsNullOrWhiteSpace(asignacion.Grupo) ? "N/A" : asignacion.Grupo.Trim().ToUpper();

    //        var specialty = await _specialtyService.GetOrCreateAsync(especialidad);
    //        var areaEntity = await _areaService.GetOrCreateAsync(area);
    //        var subject = await _subjectService.GetOrCreateAsync(materia);
    //        var grade = await _gradeLevelService.GetOrCreateAsync(grado);
    //        var groupEntity = await _groupService.GetOrCreateAsync(grupo);

    //        bool yaExiste = await _academicAssignmentService.ExisteAsignacionAsync(
    //            specialty.Id, areaEntity.Id, subject.Id, grade.Id, groupEntity.Id
    //        );

    //        if (!yaExiste)
    //        {
    //            await _academicAssignmentService.CreateAsignacionAsync(
    //                specialty.Id, areaEntity.Id, subject.Id, grade.Id, groupEntity.Id
    //            );
    //            asignacionesInsertadas++;
    //        }
    //    }

    //    return Ok(new
    //    {
    //        message = asignacionesInsertadas > 0
    //            ? $"Se insertaron {asignacionesInsertadas} nuevas asignaciones."
    //            : "No se insertaron nuevas asignaciones. Todas ya existían.",
    //        success = true
    //    });
    //}

    //public async Task<IActionResult> SaveAssignmentsFromExcel([FromBody] List<AssignmentInputModel> asignaciones)
    //{
    //    if (asignaciones == null || !asignaciones.Any())
    //        return BadRequest(new { message = "No se recibió información válida." });

    //    var asignacionesInsertadas = 0;


    //    foreach (var asignacion in asignaciones)
    //    {


    //        var specialty = await _specialtyService.GetOrCreateAsync(asignacion.Especialidad);
    //        var area = await _areaService.GetOrCreateAsync(asignacion.Area);
    //        var subject = await _subjectService.GetOrCreateAsync(asignacion.Materia);
    //        var grade = await _gradeLevelService.GetOrCreateAsync(asignacion.Grado);
    //        var group = await _groupService.GetOrCreateAsync(asignacion.Grupo);

    //        // Validar si ya existe la asignación completa
    //        bool yaExiste = await _academicAssignmentService.ExisteAsignacionAsync(
    //            specialty.Id, area.Id, subject.Id, grade.Id, group.Id
    //        );

    //        if (!yaExiste)
    //        {
    //            await _academicAssignmentService.CreateAsignacionAsync(
    //                specialty.Id, area.Id, subject.Id, grade.Id, group.Id
    //            );
    //            asignacionesInsertadas++;
    //        }
    //    }

    //    return Ok(new
    //    {
    //        message = asignacionesInsertadas > 0
    //            ? $"Se insertaron {asignacionesInsertadas} nuevas asignaciones."
    //            : "No se insertaron nuevas asignaciones. Todas ya existían.",
    //        success = true
    //    });
    //}
    //// Mostrar listado de profesores y formulario de asignación
    public async Task<IActionResult> Index()
    {
        var user = await _userService.GetAllTeachersAsync();
        ViewBag.Subjects = await _subjectService.GetAllAsync();
        ViewBag.Groups = await _groupService.GetAllAsync();
        ViewBag.Grades = await _gradeLevelService.GetAllAsync();

        return View(user);
    }

    [HttpGet("Assign")]
    public async Task<IActionResult> Assign(Guid id)
    {
        var teacher = await _userService.GetByIdAsync(id);
        var subjects = (await _subjectService.GetAllAsync()).ToList();
        var grades = (await _gradeLevelService.GetAllAsync()).ToList();
        var groups = (await _groupService.GetAllAsync()).ToList();

        var viewModel = new AssignViewModel
        {
            Teacher = teacher,
            Subjects = subjects,
            Grades = grades,
            Groups = groups
        };

        return View("Assign", viewModel);
    }

    public async Task<IActionResult> GuardarAsignacion([FromBody] TeacherAssignmentRequest request)
    {
        if (request.GroupIds == null || !request.GroupIds.Any())
        {
            return BadRequest(new { success = false, message = "Debe seleccionar al menos un grupo." });
        }

        var insertedGroupIds = new List<Guid>();

        foreach (var groupId in request.GroupIds)
        {
            var subjectAssignmentId = await _academicAssignmentService.GetSubjectAssignmentIdAsync(
                request.SpecialtyId,
                request.AreaId,
                request.SubjectId,
                request.GradeId,
                groupId
            );

            if (subjectAssignmentId == null)
                continue; // No se encontró la combinación completa

            var inserted = await _academicAssignmentService.AssignTeacherAsync(
                request.UserId,
                subjectAssignmentId.Value
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
                message = "Estas combinaciones ya existen o no fueron encontradas en las asignaciones académicas."
            });
        }

        var subject = await _subjectService.GetByIdAsync(request.SubjectId);
        var grade = await _gradeLevelService.GetByIdAsync(request.GradeId);
        var allGroups = await _groupService.GetAllAsync();

        var insertedGroupNames = allGroups
            .Where(g => insertedGroupIds.Contains(g.Id))
            .Select(g => g.Name)
            .ToList();

        var response = new
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
        };

        return Ok(response);
    }



    [HttpPost]
    public async Task<IActionResult> UpdateAssignments(Guid userId, List<Guid> subjectIds, List<Guid> groupIds, List<Guid> gradeLevelIds)
    {
        var user = await _userService.GetByIdWithRelationsAsync(userId);
        if (user == null) return NotFound();

        await _userService.UpdateAsync(user, subjectIds, groupIds, gradeLevelIds);

        return Json(new { success = true, message = "Asignaciones actualizadas correctamente." });
    }

    // Métodos CRUD para TeacherAssignment (los dejamos tal como están)
    public async Task<IActionResult> Details(Guid id)
    {
        var assignment = await _teacherAssignmentService.GetByIdAsync(id);
        if (assignment == null) return NotFound();
        return View(assignment);
    }

    public IActionResult Create() => View();

    //[HttpPost]
    //public async Task<IActionResult> Create(TeacherAssignment assignment)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        await _teacherAssignmentService.CreateAsync(assignment);
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(assignment);
    //}

    public async Task<IActionResult> Edit(Guid id)
    {
        var assignment = await _teacherAssignmentService.GetByIdAsync(id);
        if (assignment == null) return NotFound();
        return View(assignment);
    }

    //[HttpPost]
    //public async Task<IActionResult> Edit(TeacherAssignment assignment)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        await _teacherAssignmentService.UpdateAsync(assignment);
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(assignment);
    //}

    public async Task<IActionResult> Delete(Guid id)
    {
        var assignment = await _teacherAssignmentService.GetByIdAsync(id);
        if (assignment == null) return NotFound();
        return View(assignment);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _teacherAssignmentService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }


}
