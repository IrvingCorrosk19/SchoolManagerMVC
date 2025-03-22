using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManager.Enums;
using SchoolManager.Models;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly ISubjectService _subjectService;
    private readonly IGroupService _groupService;

    public UserController(
        IUserService userService,
        ISubjectService subjectService,
        IGroupService groupService)
    {
        _userService = userService;
        _subjectService = subjectService;
        _groupService = groupService;
    }


    public async Task<IActionResult> Index()
    {
        ViewBag.Roles = Enum.GetNames(typeof(UserRole)).ToList();

        var subjects = await _subjectService.GetAllAsync() ?? new List<Subject>();
        ViewBag.Subjects = subjects;
        ViewBag.SubjectsSelectList = new SelectList(subjects, "Id", "Name");

        var groups = await _groupService.GetAllAsync() ?? new List<Group>();
        ViewBag.Groups = groups;
        ViewBag.GroupsSelectList = new SelectList(groups, "Id", "Name");

        var users = await _userService.GetAllAsync();
        return View(users);
    }


    public async Task<IActionResult> Details(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        if (ModelState.IsValid)
        {
            await _userService.CreateAsync(user);
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(User user)
    {
        if (ModelState.IsValid)
        {
            await _userService.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _userService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
