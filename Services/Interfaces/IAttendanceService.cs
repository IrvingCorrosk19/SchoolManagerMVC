﻿using SchoolManager.Models;
using SchoolManager.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAttendanceService
{
    Task<List<Attendance>> GetAllAsync();
    Task<Attendance?> GetByIdAsync(Guid id);
    Task CreateAsync(Attendance attendance);
    Task UpdateAsync(Attendance attendance);
    Task DeleteAsync(Guid id);
    Task<List<Attendance>> GetByStudentAsync(Guid studentId);
    Task<List<Attendance>> GetHistorialAsync(Guid groupId, Guid gradeId, DateOnly? fechaInicio, DateOnly? fechaFin, Guid? studentId = null);
    Task<EstadisticasAsistenciaDto> GetEstadisticasAsync(Guid groupId, Guid gradeId, string trimestre, DateTime fechaInicio, DateTime fechaFin);
    Task SaveAttendancesAsync(List<AttendanceSaveDto> attendances);
    Task<List<object>> GetHistorialAsistenciaAsync(HistorialAsistenciaFiltroDto filtro);
}
