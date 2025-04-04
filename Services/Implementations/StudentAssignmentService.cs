using Microsoft.EntityFrameworkCore;
using SchoolManager.Models;
using SchoolManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManager.Services.Implementations
{
    public class StudentAssignmentService : IStudentAssignmentService
    {
        private readonly SchoolDbContext _context;

        public StudentAssignmentService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentAssignment>> GetAssignmentsByStudentIdAsync(Guid studentId)
        {
            return await _context.StudentAssignments
                .Where(sa => sa.StudentId == studentId)
                .ToListAsync();
        }

        public async Task AssignAsync(Guid studentId, List<(Guid GradeId, Guid GroupId)> assignments)
        {
            var existing = await _context.StudentAssignments
                .Where(a => a.StudentId == studentId)
                .ToListAsync();

            _context.StudentAssignments.RemoveRange(existing);

            foreach (var item in assignments)
            {
                _context.StudentAssignments.Add(new StudentAssignment
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    GradeId = item.GradeId,
                    GroupId = item.GroupId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> AssignStudentAsync(Guid studentId, Guid gradeId, Guid groupId)
        {
            // Elimina cualquier asignación previa de ese estudiante
            var previousAssignments = await _context.StudentAssignments
                .Where(sa => sa.StudentId == studentId)
                .ToListAsync();

            if (previousAssignments.Any())
            {
                _context.StudentAssignments.RemoveRange(previousAssignments);
                await _context.SaveChangesAsync(); // Guardamos para liberar antes de agregar la nueva
            }

            // Crea nueva asignación
            var assignment = new StudentAssignment
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                GradeId = gradeId,
                GroupId = groupId,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };

            _context.StudentAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task RemoveAssignmentsAsync(Guid studentId)
        {
            var assignments = await _context.StudentAssignments
                .Where(a => a.StudentId == studentId)
                .ToListAsync();

            _context.StudentAssignments.RemoveRange(assignments);
            await _context.SaveChangesAsync();
        }
    }
}
