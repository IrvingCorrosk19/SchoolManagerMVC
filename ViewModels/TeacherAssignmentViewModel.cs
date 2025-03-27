using SchoolManager.Models;

namespace SchoolManager.ViewModels;

public class TeacherAssignmentViewModel
{
    // Asignaciones existentes para la tabla
    public List<TeacherAssignment> Assignments { get; set; } = new();

    // Datos del formulario (selección)
    public Guid? SelectedTeacherId { get; set; }
    public Guid? SelectedSubjectId { get; set; }
    public Guid? SelectedGroupId { get; set; }

    // Opciones para los selects
    public List<User> Teachers { get; set; } = new();
    public List<Subject> Subjects { get; set; } = new();
    public List<Group> Groups { get; set; } = new();
}
