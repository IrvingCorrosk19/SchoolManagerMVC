using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;

public class AttendanceService : IAttendanceService
{
    private readonly SchoolDbContext _context;

    public AttendanceService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Attendance>> GetAllAsync() =>
        await _context.Attendances.ToListAsync();

    public async Task<Attendance?> GetByIdAsync(Guid id) =>
        await _context.Attendances.FindAsync(id);

    public async Task CreateAsync(Attendance attendance)
    {
        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Attendance attendance)
    {
        _context.Attendances.Update(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance != null)
        {
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Attendance>> GetByStudentAsync(Guid studentId)
    {
        return await _context.Attendances
            .Where(a => a.StudentId == studentId)
            .ToListAsync();
    }
}
