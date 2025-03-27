using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;

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
            .Include(t => t.Subject)
            .Include(t => t.Group)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
    public async Task<List<TeacherAssignment>> FilterAsync(Guid? subjectId, Guid? groupId)
    {
        var query = _context.TeacherAssignments
            .Include(t => t.Teacher)
            .Include(t => t.Subject)
            .Include(t => t.Group)
            .AsQueryable();

        if (subjectId.HasValue)
            query = query.Where(a => a.SubjectId == subjectId);

        if (groupId.HasValue)
            query = query.Where(a => a.GroupId == groupId);

        return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
    }

    public async Task<TeacherAssignment?> GetByIdAsync(Guid id) =>
        await _context.TeacherAssignments.FindAsync(id);

    public async Task CreateAsync(TeacherAssignment assignment)
    {
        var exists = await _context.TeacherAssignments.AnyAsync(a =>
            a.TeacherId == assignment.TeacherId &&
            a.SubjectId == assignment.SubjectId &&
            a.GroupId == assignment.GroupId);

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
}
