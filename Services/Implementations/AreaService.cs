using Microsoft.EntityFrameworkCore;
using SchoolManager.Application.Interfaces;
using SchoolManager.Models;
using SchoolManager.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SchoolManager.Infrastructure.Services
{
    public class AreaService : IAreaService
    {
        private readonly SchoolDbContext _context;

        public AreaService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<Area> GetOrCreateAsync(string name)
        {
            name = name.Trim().ToUpper();

            var area = await _context.Areas
                .FirstOrDefaultAsync(a => a.Name.ToUpper() == name);

            if (area == null)
            {
                area = new Area
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
                };

                _context.Areas.Add(area);
                await _context.SaveChangesAsync();
            }

            return area;
        }
    }
}
