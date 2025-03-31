using Microsoft.AspNetCore.Mvc;
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

    public AcademicAssignmentController(
        ITeacherAssignmentService teacherAssignmentService,
        IUserService userService,
        ISubjectService subjectService,
        IGroupService groupService,
        IGradeLevelService gradeLevelService)
    {
        _teacherAssignmentService = teacherAssignmentService;
        _userService = userService;
        _subjectService = subjectService;
        _groupService = groupService;
        _gradeLevelService = gradeLevelService;
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

    // Mostrar asignaciones actuales de un profesor seleccionado (AJAX)
    [HttpGet]
    public async Task<IActionResult> GetAssignments(Guid userId)
    {
        var user = await _userService.GetByIdWithRelationsAsync(userId);
        if (user == null)
            return NotFound();

        var viewModel = new TeacherActivityViewModel
        {
            UserId = user.Id,
            Groups = user.Groups.OrderBy(g => g.Name).ToList(),
            Subjects = user.Subjects.OrderBy(s => s.Name).ToList(),

        };

        return PartialView("_AssignmentsPartial", viewModel); // Se cargará con AJAX
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
