using Microsoft.EntityFrameworkCore;
using SchoolManager.Models;
using SchoolManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManager.Services
{
    public class SubjectAssignmentService : ISubjectAssignmentService
    {
        private readonly SchoolDbContext _context;

        public SubjectAssignmentService(SchoolDbContext context)
        {
            _context = context;
        }
        public async Task<List<(Guid GradeLevelId, Guid GroupId)>> GetDistinctGradeGroupCombinationsAsync()
        {
            var result = await _context.SubjectAssignments
                .Select(x => new { x.GradeLevelId, x.GroupId }) // EF-friendly
                .Distinct()
                .ToListAsync(); // Aquí sí usamos ToListAsync porque aún estamos en EF

            return result
                .Select(x => (x.GradeLevelId, x.GroupId)) // en memoria: convertimos a tuplas
                .ToList(); // ✅ ToList normal porque ya es IEnumerable
        }

    }
}
