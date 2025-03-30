using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;

public class SubjectService : ISubjectService
{
    private readonly SchoolDbContext _context;

    public SubjectService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Subject>> GetAllAsync() =>
        await _context.Subjects.ToListAsync();

    public async Task<Subject?> GetByIdAsync(Guid id) =>
        await _context.Subjects.FindAsync(id);

    public async Task<Subject> CreateAsync(Subject subject)
    {
        // guardar en la base de datos
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return subject;
    }

    public async Task<Subject> UpdateAsync(Subject subject)
    {
        _context.Subjects.Update(subject);
        await _context.SaveChangesAsync();
        return subject;
    }


    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            // Puedes registrar el error si tienes un sistema de logging, por ejemplo:
            // _logger.LogError(ex, "Error al eliminar la materia con ID: {id}", id);
            throw new Exception("Error al eliminar la materia.", ex);
        }
    }

}
