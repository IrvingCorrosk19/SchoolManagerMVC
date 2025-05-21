﻿using System;
using System.Collections.Generic;

namespace SchoolManager.Models;

public partial class Activity
{
    public Guid Id { get; set; }

    public Guid? SchoolId { get; set; }

    public Guid? SubjectId { get; set; }

    public Guid? TeacherId { get; set; }

    public Guid? GroupId { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Trimester { get; set; }

    public string? PdfUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? GradeLevelId { get; set; }

    public virtual ICollection<ActivityAttachment> ActivityAttachments { get; set; } = new List<ActivityAttachment>();

    public virtual Group? Group { get; set; }

    public virtual School? School { get; set; }

    public virtual ICollection<StudentActivityScore> StudentActivityScores { get; set; } = new List<StudentActivityScore>();

    public virtual Subject? Subject { get; set; }

    public virtual User? Teacher { get; set; }
}
