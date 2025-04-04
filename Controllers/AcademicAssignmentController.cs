using Microsoft.AspNetCore.Mvc;
using SchoolManager.Application.Interfaces;
using SchoolManager.Infrastructure.Services;
using SchoolManager.Models;
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

    public AcademicAssignmentController(
        ITeacherAssignmentService teacherAssignmentService,
        IUserService userService,
        ISubjectService subjectService,
        IGroupService groupService,
        IGradeLevelService gradeLevelService,
        IAcademicAssignmentService academicAssignmentService)
    {
        _teacherAssignmentService = teacherAssignmentService;
        _userService = userService;
        _subjectService = subjectService;
        _groupService = groupService;
        _gradeLevelService = gradeLevelService;
        _academicAssignmentService= academicAssignmentService;
    }

    // Mostrar listado de profesores y formulario de asignación
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

    [HttpPost]
    public async Task<IActionResult> GuardarAsignacion([FromBody] TeacherAssignmentRequest request)
    {
        if (request.GroupIds == null || !request.GroupIds.Any())
        {
            return BadRequest(new { success = false, message = "Debe seleccionar al menos un grupo." });
        }

        var insertedGroupIds = new List<Guid>();

        foreach (var groupId in request.GroupIds)
        {
            var inserted = await _academicAssignmentService.AssignTeacherAsync(
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

    [HttpPost]
    public async Task<IActionResult> Create(TeacherAssignment assignment)
    {
        if (ModelState.IsValid)
        {
            await _teacherAssignmentService.CreateAsync(assignment);
            return RedirectToAction(nameof(Index));
        }
        return View(assignment);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var assignment = await _teacherAssignmentService.GetByIdAsync(id);
        if (assignment == null) return NotFound();
        return View(assignment);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TeacherAssignment assignment)
    {
        if (ModelState.IsValid)
        {
            await _teacherAssignmentService.UpdateAsync(assignment);
            return RedirectToAction(nameof(Index));
        }
        return View(assignment);
    }

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
