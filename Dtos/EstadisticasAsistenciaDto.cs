using System.Collections.Generic;

namespace SchoolManager.Dtos
{
    public class EstadisticasAsistenciaDto
    {
        public int TotalRegistros { get; set; }
        public int TotalPresentes { get; set; }
        public int TotalAusentes { get; set; }
        public int TotalTardanzas { get; set; }
        public decimal PorcentajeAsistencia { get; set; }
        public decimal PorcentajeAusencias { get; set; }
        public decimal PorcentajeTardanzas { get; set; }
        public List<EstadisticaEstudianteDto> PorEstudiante { get; set; } = new();
    }

    public class EstadisticaEstudianteDto
    {
        public string Estudiante { get; set; }
        public int Presentes { get; set; }
        public int Ausentes { get; set; }
        public int Tardanzas { get; set; }
        public decimal PorcentajeAsistencia { get; set; }
    }
} 