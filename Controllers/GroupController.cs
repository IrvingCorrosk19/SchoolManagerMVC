using Microsoft.AspNetCore.Mvc;
using SchoolManager.Models;

public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public async Task<IActionResult> Index()
    {
        var groups = await _groupService.GetAllAsync();
        return View(groups);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var group = await _groupService.GetByIdAsync(id);
        if (group == null) return NotFound();
        return View(group);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Group group)
    {
        if (ModelState.IsValid)
        {
            await _groupService.CreateAsync(group);
            return RedirectToAction(nameof(Index));
        }
        return View(group);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var group = await _groupService.GetByIdAsync(id);
        if (group == null) return NotFound();
        return View(group);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Group group)
    {
        if (ModelState.IsValid)
        {
            await _groupService.UpdateAsync(group);
            return RedirectToAction(nameof(Index));
        }
        return View(group);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var group = await _groupService.GetByIdAsync(id);
        if (group == null) return NotFound();
        return View(group);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _groupService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
