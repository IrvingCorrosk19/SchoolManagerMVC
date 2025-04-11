using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly SchoolDbContext _context;

    public UserService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllStudentsAsync()
    {
        return await _context.Users
            .Where(u => u.Role.ToLower() == "student" || u.Role.ToLower() == "estudiante")
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    public async Task UpdateAsync(User user, List<Guid> subjectIds, List<Guid> groupIds)
    {
        // Actualizar Subjects
        user.Subjects.Clear();
        if (subjectIds.Any())
        {
            var subjects = await _context.Subjects.Where(s => subjectIds.Contains(s.Id)).ToListAsync();
            foreach (var subject in subjects)
            {
                user.Subjects.Add(subject);
            }
        }

        // Actualizar Groups
        user.Groups.Clear();
        if (groupIds.Any())
        {
            var groups = await _context.Groups.Where(g => groupIds.Contains(g.Id)).ToListAsync();
            foreach (var group in groups)
            {
                user.Groups.Add(group);
            }
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(User user, List<Guid> subjectIds, List<Guid> groupIds, List<Guid> gradeLevelIds)
    {
        // Actualizar Subjects
        user.Subjects.Clear();
        if (subjectIds.Any())
        {
            var subjects = await _context.Subjects.Where(s => subjectIds.Contains(s.Id)).ToListAsync();
            foreach (var subject in subjects)
            {
                user.Subjects.Add(subject);
            }
        }


        // Actualizar Groups
        user.Groups.Clear();
        if (groupIds.Any())
        {
            var groups = await _context.Groups.Where(g => groupIds.Contains(g.Id)).ToListAsync();
            foreach (var group in groups)
            {
                user.Groups.Add(group);
            }
        }

        // Actualizar GradeLevels
        user.Grades.Clear();
        if (gradeLevelIds.Any())
        {
            var grades = await _context.GradeLevels.Where(g => gradeLevelIds.Contains(g.Id)).ToListAsync();
            foreach (var grade in grades)
            {
                user.Grades.Add(grade);
            }
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task<List<User>> GetAllTeachersAsync()
    {
        return await _context.Users
            .Where(u => u.Role == "teacher")
            .OrderBy(u => u.Name)
            .ToListAsync();
    }
    public async Task CreateAsync(User user, List<Guid> subjectIds, List<Guid> groupIds)
    {
        try
        {
            // Cargar las entidades completas desde la base de datos
            var subjects = await _context.Subjects.Where(s => subjectIds.Contains(s.Id)).ToListAsync();
            var groups = await _context.Groups.Where(g => groupIds.Contains(g.Id)).ToListAsync();

            // Asignar las relaciones
            user.Subjects = subjects;
            user.Groups = groups;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Aquí puedes registrar el error o lanzarlo nuevamente
            // Por ejemplo, puedes usar un logger o simplemente lanzar de nuevo
            throw new Exception("Error al crear el usuario y asignar relaciones.", ex);
        }
    }
    public async Task<User?> GetByIdWithRelationsAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Subjects)
            .Include(u => u.Groups)
            .Include(u => u.Grades)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetAllAsync() =>
        await _context.Users.ToListAsync();

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _context.Users.FindAsync(id);

    public async Task CreateAsync(User user, List<Guid> subjectIds, List<Guid> groupIds, List<Guid> gradeLevelIds)
    {
        try
        {
            var subjects = await _context.Subjects.Where(s => subjectIds.Contains(s.Id)).ToListAsync();
            var groups = await _context.Groups.Where(g => groupIds.Contains(g.Id)).ToListAsync();
            var grades = await _context.GradeLevels.Where(g => gradeLevelIds.Contains(g.Id)).ToListAsync();

            user.Subjects = subjects;
            user.Groups = groups;
            user.Grades = grades;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el usuario y asignar relaciones.", ex);
        }
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.Subjects)
            .Include(u => u.Groups)
            .Include(u => u.Grades)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return;

        user.Subjects.Clear();
        user.Groups.Clear();
        user.Grades.Clear();

        await _context.SaveChangesAsync();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == email.ToLower().Trim());
    }

}
