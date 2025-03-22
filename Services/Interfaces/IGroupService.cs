using SchoolManager.Models;
public interface IGroupService
{
    Task<List<Group>> GetAllAsync();
    Task<Group?> GetByIdAsync(Guid id);
    Task CreateAsync(Group group);
    Task UpdateAsync(Group group);
    Task DeleteAsync(Guid id);
}
