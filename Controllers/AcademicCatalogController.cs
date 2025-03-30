using Microsoft.AspNetCore.Mvc;
using SchoolManager.ViewModels;
using SchoolManager.Services.Interfaces; // Asegúrate de incluir los namespaces correctos

public class AcademicCatalogController : Controller
{
    private readonly IGradeLevelService _gradeLevelService;
    private readonly IGroupService _groupService;
    private readonly ISubjectService _subjectService;

    public AcademicCatalogController(
        IGradeLevelService gradeLevelService,
        IGroupService groupService,
        ISubjectService subjectService)
    {
        _gradeLevelService = gradeLevelService;
        _groupService = groupService;
        _subjectService = subjectService;
    }

    public async Task<IActionResult> Index()
    {
        // Asegurarse de que se utiliza el tipo GradeLevel correctamente
        var model = new AcademicCatalogViewModel
        {
            GradesLevel = await _gradeLevelService.GetAllAsync(), // Esto debe devolver IEnumerable<GradeLevel>
            Groups = await _groupService.GetAllAsync(),
            Subjects = await _subjectService.GetAllAsync()
        };

        return View(model);
    }
}
