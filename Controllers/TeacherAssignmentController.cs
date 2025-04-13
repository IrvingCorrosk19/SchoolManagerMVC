using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolManager.Dtos;
using SchoolManager.Models;
using SchoolManager.Services.Interfaces;
using SchoolManager.ViewModels;

public class TeacherAssignmentController : Controller
{
    private readonly ITeacherAssignmentService _teacherAssignmentService;
    private readonly IUserService _userService;
    private readonly ISubjectService _subjectService;
    private readonly IGroupService _groupService;
    private readonly IAreaService _areaService;
    private readonly ISpecialtyService _specialtyService;
    private readonly IGradeLevelService _gradeLevelService;
    private readonly IMapper _mapper;

    public TeacherAssignmentController(
        ITeacherAssignmentService teacherAssignmentService,
        IUserService userService,
        ISubjectService subjectService,
        IGroupService groupService,
        IAreaService areaService,
        ISpecialtyService specialtyService,
        IGradeLevelService gradeLevelService,
        IMapper mapper)
    {
        _teacherAssignmentService = teacherAssignmentService;
        _userService = userService;
        _subjectService = subjectService;
        _groupService = groupService;
        _areaService = areaService;
        _specialtyService = specialtyService;
        _gradeLevelService = gradeLevelService;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index()
    {
        // 🔹 Obtener todos los usuarios con rol "teacher", con asignaciones incluidas
        var teachers = await _userService.GetAllWithAssignmentsByRoleAsync("teacher");

        var teacherDtos = new List<TeacherAssignmentDisplayDto>();

        foreach (var teacher in teachers)
        {
            var assignments = teacher.TeacherAssignments ?? new List<TeacherAssignment>();

            var dto = new TeacherAssignmentDisplayDto
            {
                TeacherId = teacher.Id,
                FullName = teacher.Name,
                Email = teacher.Email,
                Role = teacher.Role,
                IsActive = string.Equals(teacher.Status, "active", StringComparison.OrdinalIgnoreCase),

                // 🔸 Solo si tiene asignaciones se agrupan por materia
                SubjectGroupDetails = assignments.Any()
                    ? assignments
                        .GroupBy(a => a.SubjectAssignment.Subject?.Name ?? "(Sin materia)")
                        .Select(sg => new SubjectGroupSummary
                        {
                            SubjectName = sg.Key,
                            GroupGradePairs = sg
                                .Select(x => $"{x.SubjectAssignment.Group?.Name}-{x.SubjectAssignment.GradeLevel?.Name}")
                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                .Distinct()
                                .ToList()
                        }).ToList()
                    : new List<SubjectGroupSummary>()
            };

            teacherDtos.Add(dto);
        }

        var subjects = await _subjectService.GetAllAsync();
        var groups = await _groupService.GetAllAsync();
        var areas = await _areaService.GetAllAsync();
        var specialties = await _specialtyService.GetAllAsync();
        var gradeLevels = await _gradeLevelService.GetAllAsync();

        var viewModel = new TeacherAssignmentViewModel
        {
            Subjects = subjects.OrderBy(s => s.Name).ToList(),
            Groups = groups.OrderBy(g => g.Name).ToList(),
            Areas = areas.OrderBy(a => a.Name).ToList(),
            Specialties = specialties.OrderBy(s => s.Name).ToList(),
            GradeLevels = gradeLevels.OrderBy(g => g.Name).ToList(),
            TeachersWithAssignments = teacherDtos
        };

        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(Guid TeacherId, Guid SubjectId, Guid GradeLevelId, Guid GroupId, Guid AreaId, Guid SpecialtyId)
    {
        // Aquí debes aplicar lógica para actualizar la asignación del docente
        await _teacherAssignmentService.UpdateAsync(TeacherId, SubjectId, GradeLevelId, GroupId, AreaId, SpecialtyId);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignmentsByTeacher(Guid id)
    {
        var assignments = await _teacherAssignmentService.GetAssignmentsForModalByTeacherIdAsync(id);

        var result = assignments.Select(a => new
        {
            specialty = a.SubjectAssignment.Specialty?.Name,
            area = a.SubjectAssignment.Area?.Name,
            subject = a.SubjectAssignment.Subject?.Name,
            grade = a.SubjectAssignment.GradeLevel?.Name,
            group = a.SubjectAssignment.Group?.Name
        });

        return Json(result);
    }

    //public async Task<IActionResult> Index()
    //{
    //    var teacherId = Guid.Parse("1bbb72ba-396c-4b96-bd30-8a7a3997c2fc");

    //    var teacher = await _userService.GetByIdWithRelationsAsync(teacherId);
    //    if (teacher == null)
    //        return NotFound("Docente no encontrado.");

    //    var allAssignments = await _teacherAssignmentService.GetAllWithIncludesAsync();

    //    var grouped = allAssignments
    //        .GroupBy(a => a.Teacher)
    //        .Select(g => new TeacherAssignmentDisplayDto
    //        {
    //            TeacherId = g.Key.Id,
    //            FullName = g.Key.Name,
    //            Email = g.Key.Email,
    //            Role = g.Key.Role,
    //            IsActive = string.Equals(g.Key.Status, "Activo", StringComparison.OrdinalIgnoreCase),

    //            SubjectGroupDetails = g
    //                .GroupBy(a => a.SubjectAssignment.Subject?.Name ?? "(Sin materia)")
    //                .Select(sg => new SubjectGroupSummary
    //                {
    //                    SubjectName = sg.Key,
    //                    GroupGradePairs = sg
    //                        .Select(x => $"{x.SubjectAssignment.Group?.Name}-{x.SubjectAssignment.GradeLevel?.Name}")
    //                        .Where(x => !string.IsNullOrWhiteSpace(x))
    //                        .Distinct()
    //                        .ToList()
    //                })
    //                .ToList()

    //        }).ToList();

    //    var subjects = await _subjectService.GetAllAsync();
    //    var groups = await _groupService.GetAllAsync();
    //    var areas = await _areaService.GetAllAsync();
    //    var specialties = await _specialtyService.GetAllAsync();
    //    var gradeLevels = await _gradeLevelService.GetAllAsync();

    //    var viewModel = new TeacherAssignmentViewModel
    //    {
    //        SelectedTeacherId = teacher.Id,
    //        Subjects = subjects.OrderBy(s => s.Name).ToList(),
    //        Groups = groups.OrderBy(g => g.Name).ToList(),
    //        Areas = areas.OrderBy(a => a.Name).ToList(),
    //        Specialties = specialties.OrderBy(s => s.Name).ToList(),
    //        GradeLevels = gradeLevels.OrderBy(g => g.Name).ToList(),
    //        TeachersWithAssignments = grouped
    //    };

    //    return View(viewModel);
    //}


    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _teacherAssignmentService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, TeacherAssignmentViewModel model)
    {
        if (!ModelState.IsValid || model.SelectedSubjectId == null || model.SelectedGroupId == null ||
            model.SelectedGradeLevelId == null || model.SelectedAreaId == null || model.SelectedSpecialtyId == null)
        {
            return BadRequest("Datos inválidos para la edición.");
        }

        await _teacherAssignmentService.UpdateAsync(
            id,
            model.SelectedSubjectId.Value,
            model.SelectedGroupId.Value,
            model.SelectedGradeLevelId.Value,
            model.SelectedAreaId.Value,
            model.SelectedSpecialtyId.Value
        );

        return RedirectToAction(nameof(Index));
    }
}
