using SchoolManager.Models;

public interface IActivityService
{
    Task<List<Activity>> GetAllAsync();
    Task<Activity?> GetByIdAsync(Guid id);
    Task CreateAsync(Activity activity);
    Task UpdateAsync(Activity activity);
    Task DeleteAsync(Guid id);
    Task<List<Activity>> GetByGroupAndSubjectAsync(Guid groupId, Guid subjectId);
}
