using SchoolManager.Models;

namespace SchoolManager.Services.Interfaces
{
    public interface IAreaService
    {
        Task<Area> GetOrCreateAsync(string name);
    }
}
