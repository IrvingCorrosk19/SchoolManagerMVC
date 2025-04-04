using Microsoft.EntityFrameworkCore;
using SchoolManager.Application.Interfaces;
using SchoolManager.Models;
using SchoolManager.ViewModels;

namespace SchoolManager.Infrastructure.Services
{
    public class AcademicAssignmentService : IAcademicAssignmentService
    {
        private readonly SchoolDbContext _context;

        public AcademicAssignmentService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AssignTeacherAsync(Guid teacherId, Guid subjectId, Guid gradeId, Guid groupId)
        {
            try
            {
                var exists = await _context.TeacherAssignments.AnyAsync(a =>
                    a.TeacherId == teacherId &&
                    a.SubjectId == subjectId &&
                    a.GradeId == gradeId &&
                    a.GroupId == groupId);

                if (exists)
                    return false;

                var assignment = new TeacherAssignment
                {
                    Id = Guid.NewGuid(),
                    TeacherId = teacherId,
                    SubjectId = subjectId,
                    GradeId = gradeId,
                    GroupId = groupId,
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
                };

                _context.TeacherAssignments.Add(assignment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TeacherAssignmentRequest>> GetAssignmentsByTeacherAsync(Guid teacherId)
        {
            var groupedAssignments = await _context.TeacherAssignments
                .Where(a => a.TeacherId == teacherId)
                .GroupBy(a => new { a.SubjectId, a.GradeId })
                .Select(g => new TeacherAssignmentRequest
                {
                    UserId = teacherId,
                    SubjectId = g.Key.SubjectId,
                    GradeId = g.Key.GradeId,
                    GroupIds = g.Select(x => x.GroupId).ToList()
                }).ToListAsync();

            return groupedAssignments;
        }
    }
}
