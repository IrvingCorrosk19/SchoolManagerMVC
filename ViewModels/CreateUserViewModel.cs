using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManager.ViewModels
{
    public class CreateUserViewModel
    {
        public Guid Id { get; set; } // ← AÑADIR ESTA LÍNEA
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser un correo válido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Status { get; set; } = null!;

        public List<Guid> Subjects { get; set; } = new();
        public List<Guid> Groups { get; set; } = new();
    }
}
