using Microsoft.AspNetCore.Mvc;
using SchoolManager.Models;

public class ActivityController : Controller
{
    private readonly IActivityService _activityService;

    public ActivityController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    public async Task<IActionResult> Index()
    {
        var activities = await _activityService.GetAllAsync();
        return View(activities);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var activity = await _activityService.GetByIdAsync(id);
        if (activity == null) return NotFound();
        return View(activity);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Activity activity)
    {
        if (ModelState.IsValid)
        {
            await _activityService.CreateAsync(activity);
            return RedirectToAction(nameof(Index));
        }
        return View(activity);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var activity = await _activityService.GetByIdAsync(id);
        if (activity == null) return NotFound();
        return View(activity);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Activity activity)
    {
        if (ModelState.IsValid)
        {
            await _activityService.UpdateAsync(activity);
            return RedirectToAction(nameof(Index));
        }
        return View(activity);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var activity = await _activityService.GetByIdAsync(id);
        if (activity == null) return NotFound();
        return View(activity);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _activityService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
