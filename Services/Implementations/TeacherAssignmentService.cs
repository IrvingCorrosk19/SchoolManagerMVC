using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;
using SchoolManager.ViewModels;


public class TeacherAssignmentService : ITeacherAssignmentService
{
    private readonly SchoolDbContext _context;

    public TeacherAssignmentService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<TeacherAssignment>> GetAllWithIncludesAsync()
{
    return await _context.TeacherAssignments
        .Include(ta => ta.Teacher)
        .Include(ta => ta.SubjectAssignment)
            .ThenInclude(sa => sa.Subject)
        .Include(ta => ta.SubjectAssignment.Group)
        .Include(ta => ta.SubjectAssignment.GradeLevel)
        .Include(ta => ta.SubjectAssignment.Area)
        .Include(ta => ta.SubjectAssignment.Specialty)
        .ToListAsync();
}
    public async Task<List<TeacherAssignment>> GetAssignmentsForModalByTeacherIdAsync(Guid teacherId)
    {
        return await _context.TeacherAssignments
            .Include(t => t.SubjectAssignment)
                .ThenInclude(sa => sa.Subject)
            .Include(t => t.SubjectAssignment.Area)
            .Include(t => t.SubjectAssignment.Specialty)
            .Include(t => t.SubjectAssignment.GradeLevel)
            .Include(t => t.SubjectAssignment.Group)
            .Where(t => t.TeacherId == teacherId)
            .ToListAsync();
    }

    public async Task<List<TeacherAssignment>> GetByTeacherIdAsync(Guid teacherId)
    {
        return await _context.TeacherAssignments
            .Include(ta => ta.SubjectAssignment)
                .ThenInclude(sa => sa.Subject)
            .Include(ta => ta.SubjectAssignment.Group)
            .Include(ta => ta.SubjectAssignment.Area)
            .Include(ta => ta.SubjectAssignment.Specialty)
            .Include(ta => ta.SubjectAssignment.GradeLevel)
            .Where(ta => ta.TeacherId == teacherId)
            .ToListAsync();
    }

    public async Task CreateAsync(Guid teacherId, Guid subjectId, Guid groupId, Guid gradeLevelId, Guid areaId, Guid specialtyId)
    {
        var subjectAssignment = await GetOrCreateSubjectAssignment(subjectId, groupId, gradeLevelId, areaId, specialtyId);

        var exists = await _context.TeacherAssignments.AnyAsync(ta =>
            ta.TeacherId == teacherId &&
            ta.SubjectAssignmentId == subjectAssignment.Id);

        if (!exists)
        {
            var newAssignment = new TeacherAssignment
            {
                Id = Guid.NewGuid(),
                TeacherId = teacherId,
                SubjectAssignmentId = subjectAssignment.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.TeacherAssignments.Add(newAssignment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Guid assignmentId, Guid subjectId, Guid groupId, Guid gradeLevelId, Guid areaId, Guid specialtyId)
    {
        var assignment = await _context.TeacherAssignments.FindAsync(assignmentId);
        if (assignment == null)
            throw new InvalidOperationException("Asignación no encontrada.");

        var subjectAssignment = await GetOrCreateSubjectAssignment(subjectId, groupId, gradeLevelId, areaId, specialtyId);

        assignment.SubjectAssignmentId = subjectAssignment.Id;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid assignmentId)
    {
        var assignment = await _context.TeacherAssignments.FindAsync(assignmentId);
        if (assignment != null)
        {
            _context.TeacherAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<TeacherAssignment?> GetByIdAsync(Guid id)
    {
        return await _context.TeacherAssignments
            .Include(ta => ta.SubjectAssignment)
            .FirstOrDefaultAsync(ta => ta.Id == id);
    }

    private async Task<SubjectAssignment> GetOrCreateSubjectAssignment(Guid subjectId, Guid groupId, Guid gradeLevelId, Guid areaId, Guid specialtyId)
    {
        var existing = await _context.SubjectAssignments.FirstOrDefaultAsync(sa =>
            sa.SubjectId == subjectId &&
            sa.GroupId == groupId &&
            sa.GradeLevelId == gradeLevelId &&
            sa.AreaId == areaId &&
            sa.SpecialtyId == specialtyId
        );

        if (existing != null)
            return existing;

        var newAssignment = new SubjectAssignment
        {
            Id = Guid.NewGuid(),
            SubjectId = subjectId,
            GroupId = groupId,
            GradeLevelId = gradeLevelId,
            AreaId = areaId,
            SpecialtyId = specialtyId,
            CreatedAt = DateTime.UtcNow
        };

        _context.SubjectAssignments.Add(newAssignment);
        await _context.SaveChangesAsync();

        return newAssignment;
    }
}
