namespace SchoolManager.ViewModels
{
    public class TeacherAssignmentRequest
    {
        public Guid UserId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid GradeId { get; set; }
        public List<Guid> GroupIds { get; set; }
    }
}
