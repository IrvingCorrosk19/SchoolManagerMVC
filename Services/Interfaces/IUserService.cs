﻿using SchoolManager.Models;

public interface IUserService
{
    Task<User?> GetByEmailAsync(string email);

    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByIdWithRelationsAsync(Guid id);

    Task CreateAsync(User user, List<Guid> subjectIds, List<Guid> groupIds, List<Guid> gradeLevelIds);
    Task CreateAsync(User user, List<Guid> subjectIds, List<Guid> groupIds);
    Task UpdateAsync(User user, List<Guid> subjectIds, List<Guid> groupIds, List<Guid> gradeLevelIds);
    Task UpdateAsync(User user, List<Guid> subjectIds, List<Guid> groupIds);
    Task<List<User>> GetAllStudentsAsync();
    Task<List<User>> GetAllWithAssignmentsByRoleAsync(string role);

    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<User?> AuthenticateAsync(string email, string password);
    Task<List<User>> GetAllTeachersAsync();
}
