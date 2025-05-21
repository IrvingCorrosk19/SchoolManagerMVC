using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Dtos;

public class DisciplineReportService : IDisciplineReportService
{
    private readonly SchoolDbContext _context;

    public DisciplineReportService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<DisciplineReport>> GetAllAsync() =>
        await _context.DisciplineReports.ToListAsync();

    public async Task<DisciplineReport?> GetByIdAsync(Guid? id)
    {
        if (!id.HasValue)
            return null;
        
        return await _context.DisciplineReports.FindAsync(id.Value);
    }

    public async Task CreateAsync(DisciplineReport report)
    {
        _context.DisciplineReports.Add(report);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DisciplineReport report)
    {
        _context.DisciplineReports.Update(report);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var report = await _context.DisciplineReports.FindAsync(id);
        if (report != null)
        {
            _context.DisciplineReports.Remove(report);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<DisciplineReport>> GetByStudentAsync(Guid studentId)
    {
        return await _context.DisciplineReports
            .Where(r => r.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<List<DisciplineReport>> GetFilteredAsync(DateTime? fechaInicio, DateTime? fechaFin, Guid? gradoId)
    {
        var query = _context.DisciplineReports
            .Include(r => r.Student)
            .Include(r => r.Teacher)
            .AsQueryable();

        if (fechaInicio.HasValue)
            query = query.Where(r => r.Date >= DateOnly.FromDateTime(fechaInicio.Value));
        if (fechaFin.HasValue)
            query = query.Where(r => r.Date <= DateOnly.FromDateTime(fechaFin.Value));
        if (gradoId.HasValue)
        {
            // Buscar estudiantes que pertenecen al grado especificado
            var studentsInGrade = await _context.StudentAssignments
                .Where(sa => sa.GradeId == gradoId)
                .Select(sa => sa.StudentId)
                .ToListAsync();

            query = query.Where(r => r.StudentId.HasValue && studentsInGrade.Contains(r.StudentId.Value));
        }

        return await query.ToListAsync();
    }

    public async Task<List<DisciplineReportDto>> GetByStudentDtoAsync(Guid studentId, string trimester = null)
    {
        var query = _context.DisciplineReports
            .Include(r => r.Teacher)
            .Where(r => r.StudentId == studentId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(trimester))
        {
            var trimesterInfo = await _context.Trimesters
                .FirstOrDefaultAsync(t => t.Name == trimester);

            if (trimesterInfo != null)
            {
                query = query.Where(r => r.Date >= trimesterInfo.StartDate && r.Date <= trimesterInfo.EndDate);
            }
        }

        var reports = await query
            .OrderByDescending(r => r.Date)
            .ThenByDescending(r => r.CreatedAt)
            .ToListAsync();

        return reports.Select(r => new DisciplineReportDto
        {
            Type = r.ReportType,
            Status = r.Status,
            Description = r.Description,
            Date = r.Date.ToDateTime(r.Hora ?? TimeOnly.MinValue),
            Time = r.Hora?.ToString("HH:mm"),
            Teacher = r.Teacher != null ? $"{r.Teacher.Name} {r.Teacher.LastName}" : null
        }).ToList();
    }
}
