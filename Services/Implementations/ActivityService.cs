using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;

public class ActivityService : IActivityService
{
    private readonly SchoolDbContext _context;

    public ActivityService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Activity>> GetAllAsync() =>
        await _context.Activities.ToListAsync();

    public async Task<Activity?> GetByIdAsync(Guid id) =>
        await _context.Activities.FindAsync(id);

    public async Task CreateAsync(Activity activity)
    {
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Activity activity)
    {
        _context.Activities.Update(activity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity != null)
        {
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Activity>> GetByGroupAndSubjectAsync(Guid groupId, Guid subjectId)
    {
        return await _context.Activities
            .Where(a => a.GroupId == groupId && a.SubjectId == subjectId)
            .ToListAsync();
    }
}
