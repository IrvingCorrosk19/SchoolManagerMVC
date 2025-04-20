using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Dtos;
using SchoolManager.Interfaces;
using SchoolManager.Models;          // SchoolDbContext

namespace SchoolManager.Services
{
    public class TrimesterService : ITrimesterService
    {
        private readonly SchoolDbContext _context;

        public TrimesterService(SchoolDbContext context) => _context = context;

        public async Task<IEnumerable<TrimesterDto>> GetAllAsync()
        {
            return await _context.Trimesters               // tabla generada por Scaffold
                .OrderBy(t => t.StartDate)
                .Select(t => new TrimesterDto { Id = t.Id, Name = t.Name })
                .ToListAsync();
        }
    }
}
