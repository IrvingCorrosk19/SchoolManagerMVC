using SchoolManager.Models;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task CreateAsync(User user, List<Guid> subjectIds, List<Guid> groupIds);
    Task<User?> GetByIdWithRelationsAsync(Guid id);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<User?> AuthenticateAsync(string email, string password);
}
