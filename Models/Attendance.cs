using System;
using System.Collections.Generic;

namespace SchoolManager.Models;

public partial class Attendance
{
    public Guid Id { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? TeacherId { get; set; }

    public DateOnly Date { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Student? Student { get; set; }

    public virtual User? Teacher { get; set; }
}
