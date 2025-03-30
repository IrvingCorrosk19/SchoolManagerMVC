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
        await _context.Attendance.ToListAsync();

    public async Task<Attendance?> GetByIdAsync(Guid id) =>
        await _context.Attendance.FindAsync(id);

    public async Task CreateAsync(Attendance attendance)
    {
        _context.Attendance.Add(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Attendance attendance)
    {
        _context.Attendance.Update(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var attendance = await _context.Attendance.FindAsync(id);
        if (attendance != null)
        {
            _context.Attendance.Remove(attendance);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Attendance>> GetByStudentAsync(Guid studentId)
    {
        return await _context.Attendance
            .Where(a => a.StudentId == studentId)
            .ToListAsync();
    }
}
