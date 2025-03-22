using System;
using System.Collections.Generic;

namespace SchoolManager.Models;

public partial class TeacherAssignment
{
    public Guid Id { get; set; }

    public Guid? TeacherId { get; set; }

    public Guid? SubjectId { get; set; }

    public Guid? GroupId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Group? Group { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual User? Teacher { get; set; }
}
