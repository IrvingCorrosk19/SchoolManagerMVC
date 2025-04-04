using SchoolManager.ViewModels;
using System.Threading.Tasks;

namespace SchoolManager.Application.Interfaces
{
    public interface IAcademicAssignmentService
    {
        Task <bool> AssignTeacherAsync(Guid teacherId, Guid subjectId, Guid gradeId, Guid groupId);
        Task<List<TeacherAssignmentRequest>> GetAssignmentsByTeacherAsync(Guid teacherId);
    }
}
