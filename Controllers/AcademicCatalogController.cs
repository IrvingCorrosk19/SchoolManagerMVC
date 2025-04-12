using Microsoft.AspNetCore.Mvc;
using SchoolManager.ViewModels;
using SchoolManager.Services.Interfaces; // Asegúrate de incluir los namespaces correctos

public class AcademicCatalogController : Controller
{
    private readonly IGradeLevelService _gradeLevelService;
    private readonly IGroupService _groupService;
    private readonly ISubjectService _subjectService;
    private readonly IAreaService _areaService;
    private readonly ISpecialtyService _specialtyService;

    public AcademicCatalogController(
        IGradeLevelService gradeLevelService,
        IGroupService groupService,
        ISubjectService subjectService,
        IAreaService areaService,
        ISpecialtyService specialtyService)
    {
        _gradeLevelService = gradeLevelService;
        _groupService = groupService;
        _subjectService = subjectService;
        _areaService = areaService;
        _specialtyService = specialtyService;
    }

    public async Task<IActionResult> Index()
    {
        // Asegurarse de que se utiliza el tipo GradeLevel correctamente
        var model = new AcademicCatalogViewModel
        {
            GradesLevel = await _gradeLevelService.GetAllAsync(), // Esto debe devolver IEnumerable<GradeLevel>
            Groups = await _groupService.GetAllAsync(),
            Subjects = await _subjectService.GetAllAsync(),
            Areas = await _areaService.GetAllAsync(),
            Specialties = await _specialtyService.GetAllAsync()


        };

        return View(model);
    }
}
