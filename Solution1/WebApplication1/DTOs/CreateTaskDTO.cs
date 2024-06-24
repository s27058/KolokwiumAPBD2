namespace WebApplication1.DTOs;

public class CreateTaskDTO
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int IdProject { get; set; }

    public int IdReporter { get; set; }

    public int? IdAssignee { get; set; }
}