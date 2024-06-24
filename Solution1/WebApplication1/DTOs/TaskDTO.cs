namespace WebApplication1.DTOs;

public class TaskDTO
{
    public int IdTask { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int IdProject { get; set; }

    public int IdReporter { get; set; }
    
    public BasicUserDTO Reporter { get; set; }

    public int IdAssignee { get; set; }
    
    public BasicUserDTO Assignee { get; set; }
}