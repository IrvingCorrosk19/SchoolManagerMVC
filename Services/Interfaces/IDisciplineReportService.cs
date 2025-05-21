﻿using SchoolManager.Models;
using SchoolManager.Dtos;

public interface IDisciplineReportService
{
    Task<List<DisciplineReport>> GetAllAsync();
    Task<DisciplineReport?> GetByIdAsync(Guid? id);
    Task CreateAsync(DisciplineReport report);
    Task UpdateAsync(DisciplineReport report);
    Task DeleteAsync(Guid id);
    Task<List<DisciplineReport>> GetByStudentAsync(Guid studentId);
    Task<List<DisciplineReport>> GetFilteredAsync(DateTime? fechaInicio, DateTime? fechaFin, Guid? gradoId);
    Task<List<DisciplineReportDto>> GetByStudentDtoAsync(Guid studentId, string trimester = null);
}
