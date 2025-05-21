using SchoolManager.Services.Interfaces;

namespace SchoolManager.Services.Implementations
{
    public class MenuService : IMenuService
    {
        public async Task<List<MenuItem>> GetMenuItemsForUserAsync(string role)
        {
            var allMenuItems = new List<MenuItem>
            {
                new MenuItem 
                { 
                    Title = "Dashboard", 
                    Icon = "fas fa-tachometer-alt",
                    Url = "/Home/Index",
                    RequiredRoles = new[] { "admin", "teacher", "student", "director", "superadmin", "estudiante" }
                },
                new MenuItem 
                { 
                    Title = "Estudiantes", 
                    Icon = "fas fa-user-graduate",
                    Url = "/Student/Index",
                    RequiredRoles = new[] { "student", "estudiante" }
                },
                new MenuItem 
                { 
                    Title = "Portal Docente", 
                    Icon = "fas fa-chalkboard-teacher",
                    Url = "/TeacherGradebook/Index",
                    RequiredRoles = new[] { "teacher" }
                },
                new MenuItem 
                { 
                    Title = "Portal Director", 
                    Icon = "fas fa-user-tie",
                    Url = "/Director/Index",
                    RequiredRoles = new[] { "director" }
                },
                new MenuItem 
                { 
                    Title = "Catálogo de Asignaciones", 
                    Icon = "fas fa-clipboard",
                    Url = "/SubjectAssignment/Index",
                    RequiredRoles = new[] { "admin" }
                },
                new MenuItem 
                { 
                    Title = "Administración", 
                    Icon = "fas fa-cogs",
                    Url = "#",
                    RequiredRoles = new[] { "admin" },
                    SubItems = new List<MenuItem>
                    {
                        new MenuItem 
                        { 
                            Title = "Administrar Usuarios", 
                            Icon = "fas fa-users",
                            Url = "/User/Index",
                            RequiredRoles = new[] { "admin" }
                        },
                        new MenuItem 
                        { 
                            Title = "Catálogo Académico", 
                            Icon = "fas fa-layer-group",
                            Url = "/AcademicCatalog/Index",
                            RequiredRoles = new[] { "admin" }
                        },
                        new MenuItem 
                        { 
                            Title = "Asignar Docentes", 
                            Icon = "fas fa-tasks",
                            Url = "/TeacherAssignment/Index",
                            RequiredRoles = new[] { "admin" }
                        },
                        new MenuItem 
                        { 
                            Title = "Asignar Estudiantes", 
                            Icon = "fas fa-tasks",
                            Url = "/StudentAssignment/Overview",
                            RequiredRoles = new[] { "admin" }
                        },
                        new MenuItem 
                        { 
                            Title = "Carga Asignaciones Docentes", 
                            Icon = "fas fa-file-upload",
                            Url = "/AcademicAssignment/Upload",
                            RequiredRoles = new[] { "admin" }
                        },
                        new MenuItem 
                        { 
                            Title = "Carga Asignaciones Estudiantes", 
                            Icon = "fas fa-file-upload",
                            Url = "/StudentAssignment/Upload",
                            RequiredRoles = new[] { "admin" }
                        }
                    }
                }
            };

            return allMenuItems
                .Where(m => m.RequiredRoles.Contains(role.ToLower()))
                .ToList();
        }
    }
} 