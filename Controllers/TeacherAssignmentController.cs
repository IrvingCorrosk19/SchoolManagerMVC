using Microsoft.AspNetCore.Mvc;
using SchoolManager.Models;

public class TeacherAssignmentController : Controller
{
    private readonly ITeacherAssignmentService _teacherAssignmentService;

    public TeacherAssignmentController(ITeacherAssignmentService teacherAssignmentService)
    {
        _teacherAssignmentService = teacherAssignmentService;
    }

    public async Task<IActionResult> Index()
    {
        var assignments = await _teacherAssignmentService.GetAllAsync();
        return View(assignments);
    }

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
