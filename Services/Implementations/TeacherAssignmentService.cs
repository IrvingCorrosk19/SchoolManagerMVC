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

    public async Task<List<TeacherAssignment>> GetAllAsync()
    {
        return await _context.TeacherAssignments
            .Include(t => t.Teacher)
            .Include(t => t.SubjectAssignment)
                .ThenInclude(sa => sa.Subject)
            .Include(t => t.SubjectAssignment.GradeLevel)
            .Include(t => t.SubjectAssignment.Group)
            .Include(t => t.SubjectAssignment.Area)
            .Include(t => t.SubjectAssignment.Specialty)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<TeacherAssignment>> FilterAsync(Guid? subjectId, Guid? groupId)
    {
        var query = _context.TeacherAssignments
            .Include(t => t.Teacher)
            .Include(t => t.SubjectAssignment)
                .ThenInclude(sa => sa.Subject)
            .Include(t => t.SubjectAssignment.Group)
            .AsQueryable();

        if (subjectId.HasValue)
            query = query.Where(a => a.SubjectAssignment.SubjectId == subjectId);

        if (groupId.HasValue)
            query = query.Where(a => a.SubjectAssignment.GroupId == groupId);

        return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
    }

    public async Task<TeacherAssignment?> GetByIdAsync(Guid id)
    {
        return await _context.TeacherAssignments
            .Include(t => t.Teacher)
            .Include(t => t.SubjectAssignment)
                .ThenInclude(sa => sa.Subject)
            .Include(t => t.SubjectAssignment.GradeLevel)
            .Include(t => t.SubjectAssignment.Group)
            .Include(t => t.SubjectAssignment.Area)
            .Include(t => t.SubjectAssignment.Specialty)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task CreateAsync(TeacherAssignment assignment)
    {
        var exists = await _context.TeacherAssignments.AnyAsync(a =>
            a.TeacherId == assignment.TeacherId &&
            a.SubjectAssignmentId == assignment.SubjectAssignmentId);

        if (exists)
            throw new Exception("Esta asignación ya existe.");

        _context.TeacherAssignments.Add(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TeacherAssignment assignment)
    {
        _context.TeacherAssignments.Update(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var assignment = await _context.TeacherAssignments.FindAsync(id);
        if (assignment != null)
        {
            _context.TeacherAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
        }
    }


    public async Task<bool> AssignTeacherAsync(TeacherAssignmentRequest request)
    {
        foreach (var groupId in request.GroupIds)
        {
            // Buscar la asignación en subject_assignments
            var subjectAssignment = await _context.SubjectAssignments.FirstOrDefaultAsync(sa =>
                sa.SubjectId == request.SubjectId &&
                sa.GradeLevelId == request.GradeId &&
                sa.GroupId == groupId &&
                sa.AreaId == request.AreaId &&
                sa.SpecialtyId == request.SpecialtyId
            );

            if (subjectAssignment == null)
                continue; // Se puede loguear si lo deseas

            // Verificar si ya está asignado
            bool yaExiste = await _context.TeacherAssignments.AnyAsync(ta =>
                ta.TeacherId == request.UserId &&
                ta.SubjectAssignmentId == subjectAssignment.Id
            );

            if (!yaExiste)
            {
                var nueva = new TeacherAssignment
                {
                    Id = Guid.NewGuid(),
                    TeacherId = request.UserId,
                    SubjectAssignmentId = subjectAssignment.Id,
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
                };

                _context.TeacherAssignments.Add(nueva);
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

}
