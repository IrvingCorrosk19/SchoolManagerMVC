using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolManager.Dtos;

namespace SchoolManager.Interfaces
{
    public interface IStudentActivityScoreService
    {
        Task SaveAsync(IEnumerable<StudentActivityScoreCreateDto> scores);
        Task<GradeBookDto> GetGradeBookAsync(Guid teacherId, Guid groupId, string trimesterCode);
    }
}
