using SchoolManager.Models;

public interface IStudentAssignmentService
{
    Task<List<StudentAssignment>> GetAssignmentsByStudentIdAsync(Guid studentId);

    Task AssignAsync(Guid studentId, List<(Guid SubjectId, Guid GradeId, Guid GroupId)> assignments);

    Task<bool> AssignStudentAsync(Guid studentId, Guid subjectId, Guid gradeId, Guid groupId); // ← NUEVO

    Task RemoveAssignmentsAsync(Guid studentId);
}
