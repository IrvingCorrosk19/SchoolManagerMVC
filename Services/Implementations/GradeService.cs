using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;

public class GradeService : IGradeService
{
    private readonly SchoolDbContext _context;

    public GradeService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Grade>> GetAllAsync() =>
        await _context.Grades.ToListAsync();

    public async Task<Grade?> GetByIdAsync(Guid id) =>
        await _context.Grades.FindAsync(id);

    public async Task CreateAsync(Grade grade)
    {
        try
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el grado. Detalles: " + ex.Message, ex);
        }
    }


    public async Task UpdateAsync(Grade grade)
    {
        _context.Grades.Update(grade);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var grade = await _context.Grades.FindAsync(id);
        if (grade != null)
        {
            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Grade>> GetByStudentAsync(Guid studentId)
    {
        return await _context.Grades
            .Where(g => g.StudentId == studentId)
            .ToListAsync();
    }

}
