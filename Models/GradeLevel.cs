using System;
using System.Collections.Generic;

namespace SchoolManager.Models;

public partial class GradeLevel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }
}
