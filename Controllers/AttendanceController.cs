using Microsoft.AspNetCore.Mvc;
using SchoolManager.Models;

public class AttendanceController : Controller
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    public async Task<IActionResult> Index()
    {
        var attendances = await _attendanceService.GetAllAsync();
        return View(attendances);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var attendance = await _attendanceService.GetByIdAsync(id);
        if (attendance == null) return NotFound();
        return View(attendance);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Attendance attendance)
    {
        if (ModelState.IsValid)
        {
            await _attendanceService.CreateAsync(attendance);
            return RedirectToAction(nameof(Index));
        }
        return View(attendance);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var attendance = await _attendanceService.GetByIdAsync(id);
        if (attendance == null) return NotFound();
        return View(attendance);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Attendance attendance)
    {
        if (ModelState.IsValid)
        {
            await _attendanceService.UpdateAsync(attendance);
            return RedirectToAction(nameof(Index));
        }
        return View(attendance);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var attendance = await _attendanceService.GetByIdAsync(id);
        if (attendance == null) return NotFound();
        return View(attendance);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _attendanceService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
