using Microsoft.AspNetCore.Mvc;
using SchoolManager.Models;

public class SubjectController : Controller
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    public async Task<IActionResult> Index()
    {
        var subjects = await _subjectService.GetAllAsync();
        return View(subjects);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var subject = await _subjectService.GetByIdAsync(id);
        if (subject == null) return NotFound();
        return View(subject);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Subject subject)
    {
        if (ModelState.IsValid)
        {
            await _subjectService.CreateAsync(subject);
            return RedirectToAction(nameof(Index));
        }
        return View(subject);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var subject = await _subjectService.GetByIdAsync(id);
        if (subject == null) return NotFound();
        return View(subject);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Subject subject)
    {
        if (ModelState.IsValid)
        {
            await _subjectService.UpdateAsync(subject);
            return RedirectToAction(nameof(Index));
        }
        return View(subject);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var subject = await _subjectService.GetByIdAsync(id);
        if (subject == null) return NotFound();
        return View(subject);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _subjectService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
