﻿using Microsoft.AspNetCore.Http;

namespace SchoolManager.Dtos
{
    public class ActivityCreateDto
    {
        public Guid TeacherAssignmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;   // 'tarea' | 'parcial' | 'examen'
        public string TrimesterCode { get; set; } = string.Empty;   // '1T', '2T', '3T'
        public DateTime Date { get; set; }
        public IFormFile? Pdf { get; set; }
    }
}
