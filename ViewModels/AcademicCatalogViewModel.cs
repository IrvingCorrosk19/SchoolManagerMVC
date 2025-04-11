using System.Collections.Generic;
using SchoolManager.Models;

namespace SchoolManager.ViewModels
{
    public class AcademicCatalogViewModel
    {
        public IEnumerable<GradeLevel> GradesLevel { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public IEnumerable<Specialty> Specialty { get; set; }
        public IEnumerable<Area> Area { get; set; }
    }
}
