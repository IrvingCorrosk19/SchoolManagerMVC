namespace SchoolManager.Dtos
{
    public class SubjectGroupSummary
    {
        public string SubjectName { get; set; } = "";
        public List<string> GroupGradePairs { get; set; } = new();
    }

}
