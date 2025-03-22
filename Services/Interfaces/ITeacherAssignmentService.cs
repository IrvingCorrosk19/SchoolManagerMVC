using SchoolManager.Models;

public interface ITeacherAssignmentService
{
    Task<List<TeacherAssignment>> GetAllAsync();
    Task<TeacherAssignment?> GetByIdAsync(Guid id);
    Task CreateAsync(TeacherAssignment assignment);
    Task UpdateAsync(TeacherAssignment assignment);
    Task DeleteAsync(Guid id);
}
