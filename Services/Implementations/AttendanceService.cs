﻿using SchoolManager.Models;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Dtos;

public class AttendanceService : IAttendanceService
{
    private readonly SchoolDbContext _context;

    public AttendanceService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Attendance>> GetAllAsync() =>
        await _context.Attendances.ToListAsync();

    public async Task<Attendance?> GetByIdAsync(Guid id) =>
        await _context.Attendances.FindAsync(id);

    public async Task CreateAsync(Attendance attendance)
    {
        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Attendance attendance)
    {
        _context.Attendances.Update(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance != null)
        {
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Attendance>> GetByStudentAsync(Guid studentId)
    {
        return await _context.Attendances
            .Where(a => a.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<List<Attendance>> GetHistorialAsync(Guid groupId, Guid gradeId, DateOnly? fechaInicio, DateOnly? fechaFin, Guid? studentId = null)
    {
        var query = _context.Attendances
            .Where(a => a.GroupId == groupId && a.GradeId == gradeId);

        if (fechaInicio.HasValue && fechaFin.HasValue)
            query = query.Where(a => a.Date >= fechaInicio.Value && a.Date <= fechaFin.Value);
        // Si no hay fechas, no se filtra por fecha

        if (studentId.HasValue && studentId.Value != Guid.Empty)
        {
            query = query.Where(a => a.StudentId == studentId);
        }

        return await query
            .Include(a => a.Student)
            .Include(a => a.Group)
            .Include(a => a.Grade)
            .OrderBy(a => a.Date)
            .ThenBy(a => a.Student.Name)
            .ToListAsync();
    }

    public async Task<EstadisticasAsistenciaDto> GetEstadisticasAsync(Guid groupId, Guid gradeId, string trimestre, DateTime fechaInicio, DateTime fechaFin)
    {
        var fechaInicioOnly = DateOnly.FromDateTime(fechaInicio);
        var fechaFinOnly = DateOnly.FromDateTime(fechaFin);

        var asistencias = await _context.Attendances
            .Include(a => a.Student)
            .Where(a => a.GroupId == groupId
                && a.GradeId == gradeId
                && a.Status != null
                && a.Date >= fechaInicioOnly
                && a.Date <= fechaFinOnly)
            .ToListAsync();

        var total = asistencias.Count;
        var totalPresentes = asistencias.Count(a => a.Status == "present");
        var totalAusentes = asistencias.Count(a => a.Status == "absent");
        var totalTardanzas = asistencias.Count(a => a.Status == "late");

        decimal porcAsistencia = total > 0 ? Math.Round((decimal)totalPresentes * 100 / total, 1) : 0;
        decimal porcAusencias = total > 0 ? Math.Round((decimal)totalAusentes * 100 / total, 1) : 0;
        decimal porcTardanzas = total > 0 ? Math.Round((decimal)totalTardanzas * 100 / total, 1) : 0;

        var porEstudiante = asistencias
            .GroupBy(a => a.Student?.Name)
            .Select(g => new EstadisticaEstudianteDto
            {
                Estudiante = g.Key ?? "-",
                Presentes = g.Count(a => a.Status == "present"),
                Ausentes = g.Count(a => a.Status == "absent"),
                Tardanzas = g.Count(a => a.Status == "late"),
                PorcentajeAsistencia = g.Count() > 0 ? Math.Round((decimal)g.Count(a => a.Status == "present") * 100 / g.Count(), 1) : 0
            })
            .OrderBy(e => e.Estudiante)
            .ToList();

        return new EstadisticasAsistenciaDto
        {
            TotalRegistros = total,
            TotalPresentes = totalPresentes,
            TotalAusentes = totalAusentes,
            TotalTardanzas = totalTardanzas,
            PorcentajeAsistencia = porcAsistencia,
            PorcentajeAusencias = porcAusencias,
            PorcentajeTardanzas = porcTardanzas,
            PorEstudiante = porEstudiante
        };
    }

    public async Task SaveAttendancesAsync(List<AttendanceSaveDto> attendances)
    {
        if (attendances == null || attendances.Count == 0)
            throw new ArgumentException("No se recibieron asistencias.");

        foreach (var dto in attendances)
        {
            var attendance = new Attendance
            {
                Id = Guid.NewGuid(),
                StudentId = dto.StudentId,
                TeacherId = dto.TeacherId,
                GroupId = dto.GroupId,
                GradeId = dto.GradeId,
                Date = dto.Date,
                Status = dto.Status,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };
            _context.Attendances.Add(attendance);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<List<object>> GetHistorialAsistenciaAsync(HistorialAsistenciaFiltroDto filtro)
    {
        if (filtro == null || filtro.GroupId == Guid.Empty || filtro.GradeId == Guid.Empty)
            throw new ArgumentException("Faltan datos para la consulta.");

        var studentId = string.IsNullOrEmpty(filtro.StudentId) ? (Guid?)null : Guid.Parse(filtro.StudentId);
        var lista = await GetHistorialAsync(
            filtro.GroupId,
            filtro.GradeId,
            filtro.FechaInicio,
            filtro.FechaFin,
            studentId
        );

        var resultado = lista.Select(a => new {
            estudiante = a.Student?.Name,
            fecha = a.Date.ToString("yyyy-MM-dd"),
            estado = a.Status,
            grupo = a.Group?.Name,
            grado = a.Grade?.Name
        }).ToList<object>();

        return resultado;
    }
}
