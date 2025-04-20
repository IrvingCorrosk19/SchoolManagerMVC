using SchoolManager.Dtos;

namespace SchoolManager.ViewModels
{
    public class TeacherGradebookViewModel
    {
        public TeacherAssignmentDisplayDto Teacher { get; set; } = null!;
        public IEnumerable<TrimesterDto> Trimesters { get; set; } = new List<TrimesterDto>();
        public IEnumerable<GroupDto> Groups { get; set; } = new List<GroupDto>();
        public IEnumerable<ActivityTypeDto> Types { get; set; } = new List<ActivityTypeDto>();
        public Guid TeacherId { get; set; }
    }
}
