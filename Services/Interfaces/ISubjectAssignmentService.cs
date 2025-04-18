using SchoolManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManager.Services.Interfaces
{
    public interface ISubjectAssignmentService
    {
        Task<List<(Guid GradeLevelId, Guid GroupId)>> GetDistinctGradeGroupCombinationsAsync();
    }
}
