using SchoolManager.Models;

namespace SchoolManager.Services.Interfaces
{
    public interface ISpecialtyService
    {
        Task<Specialty> GetOrCreateAsync(string name);
    }
}
