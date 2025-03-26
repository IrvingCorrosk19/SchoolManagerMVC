using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly SchoolDbContext _context;

    public UserService(SchoolDbContext context)
    {
        _context = context;
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

    public async Task<User?> GetByIdWithRelationsAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Subjects)
            .Include(u => u.Groups)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetAllAsync() =>
        await _context.Users.ToListAsync();

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _context.Users.FindAsync(id);

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
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return;

        // Limpiar relaciones muchos a muchos
        user.Subjects.Clear();
        user.Groups.Clear();

        await _context.SaveChangesAsync(); // Guardar después de limpiar relaciones

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(); // Ahora eliminar usuario
    }


    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);
    }
}
