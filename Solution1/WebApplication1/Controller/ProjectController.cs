using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controller;

[ApiController]
[Route("/api/")]
public class ProjectController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IProjectService _service;

    public ProjectController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskAsync([FromRoute] int? id)
    {
        if (id == null)
        {
            return Ok(await _service.GetTasksAsync());
        }
        if (!await _service.TaskExists(id.Value))
        {
            return NotFound();
        }
        return Ok(await _service.GetTaskAsync(id.Value));

    }

    [HttpPost()]
    public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskDTO dto)
    {
        if (!await _service.ProjectExists(dto.IdProject))
        {
            return NotFound("projekt nie istnieje");
        }
        if (!await _service.UserExists(dto.IdReporter))
        {
            return NotFound("reporter nie istnieje");
        }

        int idAssigneeNotNull = 0;

        if (dto.IdAssignee == null)
        {
            idAssigneeNotNull = await _service.GetDefaultAssigneeIdAsync(dto.IdProject);
        }
        else
        {
            idAssigneeNotNull = dto.IdAssignee.Value;
        }
        
        if (!await _service.UserExists(idAssigneeNotNull))
        {
            return NotFound("assignee nie istnieje");
        }
        
        if (!await _service.UserHasAccess(dto.IdReporter, dto.IdProject))
        {
            return NotFound("reporter nie ma dostepu");
        }
        
        if (!await _service.UserHasAccess(idAssigneeNotNull, dto.IdProject))
        {
            return NotFound("assignee nie ma dostepu");
        }

        await _service.CreateTask(dto.Name, dto.Description, dto.IdProject, dto.IdReporter, idAssigneeNotNull);
        return Created();
    }
}