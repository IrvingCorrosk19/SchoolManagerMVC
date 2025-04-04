using SchoolManager.Models;

public interface IStudentAssignmentService
{
    Task<List<StudentAssignment>> GetAssignmentsByStudentIdAsync(Guid studentId);

    Task AssignAsync(Guid studentId, List<(Guid GradeId, Guid GroupId)> assignments);

    Task<bool> AssignStudentAsync(Guid studentId, Guid gradeId, Guid groupId); // ← Actualizado

    Task RemoveAssignmentsAsync(Guid studentId);
}
