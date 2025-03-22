using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;

public class TeacherAssignmentService : ITeacherAssignmentService
{
    private readonly SchoolDbContext _context;

    public TeacherAssignmentService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<TeacherAssignment>> GetAllAsync() =>
        await _context.TeacherAssignments.ToListAsync();

    public async Task<TeacherAssignment?> GetByIdAsync(Guid id) =>
        await _context.TeacherAssignments.FindAsync(id);

    public async Task CreateAsync(TeacherAssignment assignment)
    {
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
