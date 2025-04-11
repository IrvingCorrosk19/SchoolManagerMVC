using Microsoft.EntityFrameworkCore;
using SchoolManager.Application.Interfaces;
using SchoolManager.Models;
using SchoolManager.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SchoolManager.Infrastructure.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly SchoolDbContext _context;

        public SpecialtyService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<Specialty> GetOrCreateAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("El nombre de la especialidad no puede estar vacío.", nameof(name));

                name = name.Trim().ToUpper();
                var specialty = await _context.Specialties
                    .FirstOrDefaultAsync(e => e.Name.ToUpper() == name);

                if (specialty == null)
                {
                    specialty = new Specialty
                    {
                        Id = Guid.NewGuid(),
                        Name = name,
                        CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
                    };
                    _context.Specialties.Add(specialty);
                    await _context.SaveChangesAsync();
                }

                return specialty;
            }
            catch (DbUpdateException ex)
            {
                // Manejo específico para errores de base de datos
                // Por ejemplo, podría ocurrir si hay un problema de concurrencia o de restricciones
                //_logger.LogError(ex, $"Error al guardar la especialidad '{name}' en la base de datos");
                throw new Exception($"No se pudo guardar la especialidad: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Manejo para otras excepciones no previstas
                //_logger.LogError(ex, $"Error inesperado al obtener o crear la especialidad '{name}'");
                throw new Exception($"Error al procesar la especialidad: {ex.Message}", ex);
            }
        }
    }
}
