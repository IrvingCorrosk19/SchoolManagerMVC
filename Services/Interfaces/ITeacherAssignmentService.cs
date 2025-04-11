using SchoolManager.Models;
using SchoolManager.ViewModels;

public interface ITeacherAssignmentService
{
    Task<List<TeacherAssignment>> GetAllAsync();
    Task<TeacherAssignment?> GetByIdAsync(Guid id);
    Task CreateAsync(TeacherAssignment assignment);
    Task UpdateAsync(TeacherAssignment assignment);
    Task DeleteAsync(Guid id);
    Task<bool> AssignTeacherAsync(TeacherAssignmentRequest request);

}
