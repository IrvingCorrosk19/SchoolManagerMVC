using Microsoft.AspNetCore.Mvc;
using SchoolManager.Models;

public class GradeController : Controller
{
    private readonly IGradeService _gradeService;

    public GradeController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    public async Task<IActionResult> Index()
    {
        var grades = await _gradeService.GetAllAsync();
        return View(grades);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var grade = await _gradeService.GetByIdAsync(id);
        if (grade == null) return NotFound();
        return View(grade);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Grade grade)
    {
        if (ModelState.IsValid)
        {
            await _gradeService.CreateAsync(grade);
            return RedirectToAction(nameof(Index));
        }
        return View(grade);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var grade = await _gradeService.GetByIdAsync(id);
        if (grade == null) return NotFound();
        return View(grade);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Grade grade)
    {
        if (ModelState.IsValid)
        {
            await _gradeService.UpdateAsync(grade);
            return RedirectToAction(nameof(Index));
        }
        return View(grade);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var grade = await _gradeService.GetByIdAsync(id);
        if (grade == null) return NotFound();
        return View(grade);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _gradeService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
