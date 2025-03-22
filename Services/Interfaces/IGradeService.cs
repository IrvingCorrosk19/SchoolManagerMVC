using SchoolManager.Models;

public interface IGradeService
{
    Task<List<Grade>> GetAllAsync();
    Task<Grade?> GetByIdAsync(Guid id);
    Task CreateAsync(Grade grade);
    Task UpdateAsync(Grade grade);
    Task DeleteAsync(Guid id);
    Task<List<Grade>> GetByStudentAsync(Guid studentId);
}
