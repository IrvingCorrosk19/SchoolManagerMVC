using SchoolManager.Models;

namespace SchoolManager.ViewModels
{
    public class TeacherActivityViewModel
    {
        public Guid UserId { get; set; } = Guid.Empty;

        public List<Group> Groups { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();

        // Campos del formulario
        public string? ActivityName { get; set; }
        public string? ActivityType { get; set; }
        public IFormFile? PdfFile { get; set; }
    }

}
