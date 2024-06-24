using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Project
{
    public int IdProject { get; set; }

    public string Name { get; set; } = null!;

    public int IdDefaultAssignee { get; set; }

    public virtual User IdDefaultAssigneeNavigation { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<User> IdUsers { get; set; } = new List<User>();
}
