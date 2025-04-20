using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Dtos;
using SchoolManager.Interfaces;
using SchoolManager.Models;

namespace SchoolManager.Services
{
    public class StudentActivityScoreService : IStudentActivityScoreService
    {
        private readonly SchoolDbContext _context;
        public StudentActivityScoreService(SchoolDbContext context) => _context = context;

        /* ------------ 1. Guardar / actualizar notas ------------ */
        public async Task SaveAsync(IEnumerable<StudentActivityScoreCreateDto> scores)
        {
            foreach (var dto in scores)
            {
                var entity = await _context.StudentActivityScores
                    .FirstOrDefaultAsync(s => s.StudentId == dto.StudentId &&
                                              s.ActivityId == dto.ActivityId);

                if (entity is null)
                {
                    _context.StudentActivityScores.Add(new StudentActivityScore
                    {
                        Id = Guid.NewGuid(),
                        StudentId = dto.StudentId,
                        ActivityId = dto.ActivityId,
                        Score = dto.Score,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    entity.Score = dto.Score;
                }
            }
            await _context.SaveChangesAsync();
        }

        /* ------------ 2. Libro de calificaciones pivotado ------------ */
        public async Task<GradeBookDto> GetGradeBookAsync(Guid teacherId, Guid groupId, string trimesterCode)
        {
            /* 2.1 Cabeceras: actividades del docente en ese grupo y trimestre */
            var headers = await _context.Activities
                .Where(a => a.TeacherId == teacherId &&
                            a.GroupId == groupId &&
                            a.Trimester == trimesterCode)
                .OrderBy(a => a.CreatedAt)
                .Select(a => new ActivityHeaderDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Type = a.Type,
                    Date = a.CreatedAt ?? DateTime.UtcNow,   // ✅ conversión DateTime?
                    HasPdf = a.PdfUrl != null
                })
                .ToListAsync();

            var activityIds = headers.Select(h => h.Id).ToList();

            /* 2.2 Estudiantes asignados a ese grupo (StudentAssignments) */
            var studentIds = await _context.StudentAssignments
                .Where(sa => sa.GroupId == groupId)
                .Select(sa => sa.StudentId)
                .Distinct()
                .ToListAsync();

            var students = await _context.Students
                .Where(s => studentIds.Contains(s.Id))
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();

            /* 2.3 Notas existentes */
            var scores = await _context.StudentActivityScores
                .Where(s => activityIds.Contains(s.ActivityId))
                .ToListAsync();

            /* 2.4 Pivotar alumnos × actividades */
            var rows = students.Select(stu =>
            {
                var dict = new Dictionary<Guid, decimal?>();
                foreach (var hdr in headers)
                {
                    var score = scores.FirstOrDefault(x =>
                        x.StudentId == stu.Id && x.ActivityId == hdr.Id);
                    dict[hdr.Id] = score?.Score;
                }

                return new StudentGradeRowDto
                {
                    StudentId = stu.Id,
                    StudentName = stu.Name,
                    ScoresByActivity = dict
                };
            });

            return new GradeBookDto { Activities = headers, Rows = rows };
        }
    }
}
