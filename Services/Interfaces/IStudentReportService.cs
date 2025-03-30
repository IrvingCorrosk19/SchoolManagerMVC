using SchoolManager.Dtos;

namespace SchoolManager.Services.Interfaces
{
    public interface IStudentReportService
    {
        Task<StudentReportDto> GetStudentReportAsync(Guid studentId, string trimester);

    }
}
