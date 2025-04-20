using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolManager.Dtos;

namespace SchoolManager.Interfaces
{
    public interface ITrimesterService
    {
        Task<IEnumerable<TrimesterDto>> GetAllAsync();
    }
}
