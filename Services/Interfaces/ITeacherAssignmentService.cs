using SchoolManager.Models;

public interface ITeacherAssignmentService
{
    Task<List<TeacherAssignment>> GetByTeacherIdAsync(Guid teacherId);

    Task CreateAsync(Guid teacherId, Guid subjectId, Guid groupId, Guid gradeLevelId, Guid areaId, Guid specialtyId);

    Task UpdateAsync(Guid assignmentId, Guid subjectId, Guid groupId, Guid gradeLevelId, Guid areaId, Guid specialtyId);

    Task DeleteAsync(Guid assignmentId);

    Task<TeacherAssignment?> GetByIdAsync(Guid id);
    Task<List<TeacherAssignment>> GetAllWithIncludesAsync();

    Task<List<TeacherAssignment>> GetAssignmentsForModalByTeacherIdAsync(Guid teacherId);
}
