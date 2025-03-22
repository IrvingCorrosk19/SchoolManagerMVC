using SchoolManager.Models;

public interface IAttendanceService
{
    Task<List<Attendance>> GetAllAsync();
    Task<Attendance?> GetByIdAsync(Guid id);
    Task CreateAsync(Attendance attendance);
    Task UpdateAsync(Attendance attendance);
    Task DeleteAsync(Guid id);
    Task<List<Attendance>> GetByStudentAsync(Guid studentId);
}
