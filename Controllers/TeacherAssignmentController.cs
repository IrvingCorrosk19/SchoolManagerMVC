using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Models;
using SchoolManager.ViewModels;

public class TeacherAssignmentController : Controller
{
    private readonly ITeacherAssignmentService _teacherAssignmentService;
    private readonly IUserService _userService;
    private readonly ISubjectService _subjectService;
    private readonly IGroupService _groupService;
    private readonly IMapper _mapper;
    public TeacherAssignmentController(ITeacherAssignmentService teacherAssignmentService, IUserService userService,
        ISubjectService subjectService,
        IGroupService groupService,
        IMapper mapper)
    {
        _teacherAssignmentService = teacherAssignmentService;
        _userService = userService;
        _subjectService = subjectService;
        _groupService = groupService;
        _mapper = mapper;

    }

    public async Task<IActionResult> Index()
    {
        var userId = Guid.Parse("8879de76-1af9-411e-a594-a4afe8a0b557");
        var userTeacher = await _userService.GetByIdWithRelationsAsync(userId);

        if (userTeacher == null)
            return NotFound("Docente no encontrado");

        var viewModel = new TeacherActivityViewModel
        {
            UserId = userTeacher.Id,
            Groups = userTeacher.Groups.OrderBy(g => g.Name).ToList(),
            Subjects = userTeacher.Subjects.OrderBy(s => s.Name).ToList()
        };

        return View(viewModel);
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
