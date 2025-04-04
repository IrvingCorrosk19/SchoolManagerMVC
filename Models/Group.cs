﻿using System;
using System.Collections.Generic;

namespace SchoolManager.Models;

public partial class Group
{
    public Guid Id { get; set; }

    public Guid? SchoolId { get; set; }

    public string Name { get; set; } = null!;

    public string? Grade { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual School? School { get; set; }

    public virtual ICollection<StudentAssignment> StudentAssignments { get; set; } = new List<StudentAssignment>();

    public virtual ICollection<TeacherAssignment> TeacherAssignments { get; set; } = new List<TeacherAssignment>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
