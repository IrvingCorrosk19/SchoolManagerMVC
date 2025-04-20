namespace SchoolManager.Dtos
{
    public class StudentActivityScoreCreateDto
    {
        public Guid StudentId { get; set; }
        public Guid ActivityId { get; set; }
        public decimal Score { get; set; }
    }
}
