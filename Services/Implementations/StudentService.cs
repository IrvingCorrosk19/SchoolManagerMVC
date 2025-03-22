using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;

public class StudentService : IStudentService
{
    private readonly SchoolDbContext _context;

    public StudentService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Student>> GetAllAsync() =>
        await _context.Students.ToListAsync();

    public async Task<Student?> GetByIdAsync(Guid id) =>
        await _context.Students.FindAsync(id);

    public async Task CreateAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Student>> GetByGroupAsync(string groupName) =>
        await _context.Students
            .Where(s => s.GroupName == groupName)
            .ToListAsync();
}
