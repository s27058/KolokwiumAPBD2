using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Service;

public interface IProjectService
{
    public Task<TaskDTO> GetTaskAsync(int IdTask);
    public Task<ICollection<TaskDTO>> GetTasksAsync();
    public Task<bool> TaskExists(int id);
    public Task<bool> ProjectExists(int id);
    public Task<bool> UserExists(int id);
    public Task<bool> UserHasAccess(int IdUser, int IdProject);
    public Task<bool> CreateTask(string name, string description, int idProject, int idReporter, int idAssignee);
    public Task<int> GetDefaultAssigneeIdAsync(int idProject);
}

public class ProjectService : IProjectService
{
    private readonly KolokwiumContext _db;

    public ProjectService(KolokwiumContext db)
    {
        _db = db;
    }

    public async Task<TaskDTO> GetTaskAsync(int IdTask)
    {
        Models.Task task = await _db.Tasks.Include(task => task.IdAssigneeNavigation)
            .Include(task => task.IdReporterNavigation)
            .Where(task => task.IdTask == IdTask).FirstOrDefaultAsync();

        TaskDTO taskDto = new TaskDTO()
        {
            IdTask = task.IdTask,
            Description = task.Description,
            Assignee = new BasicUserDTO()
            {
                FirstName = task.IdAssigneeNavigation.FirstName,
                LastName = task.IdAssigneeNavigation.LastName
            },
            CreatedAt = task.CreatedAt,
            IdAssignee = task.IdAssignee,
            IdProject = task.IdProject,
            IdReporter = task.IdReporter,
            Name = task.Name,
            Reporter = new BasicUserDTO()
            {
                FirstName = task.IdReporterNavigation.FirstName,
                LastName = task.IdReporterNavigation.LastName
            }
        };
        return taskDto;
    }
    public async Task<ICollection<TaskDTO>> GetTasksAsync()
    {
        List<Models.Task> tasks = await _db.Tasks.Include(task => task.IdAssigneeNavigation)
            .Include(task => task.IdReporterNavigation)
            .ToListAsync();

        List<TaskDTO> results = new List<TaskDTO>();

        foreach (Models.Task task in tasks)
        {
            TaskDTO taskDto = new TaskDTO()
            {
                IdTask = task.IdTask,
                Description = task.Description,
                Assignee = new BasicUserDTO()
                {
                    FirstName = task.IdAssigneeNavigation.FirstName,
                    LastName = task.IdAssigneeNavigation.LastName
                },
                CreatedAt = task.CreatedAt,
                IdAssignee = task.IdAssignee,
                IdProject = task.IdProject,
                IdReporter = task.IdReporter,
                Name = task.Name,
                Reporter = new BasicUserDTO()
                {
                    FirstName = task.IdReporterNavigation.FirstName,
                    LastName = task.IdReporterNavigation.LastName
                }
            };
            results.Add(taskDto);
        }
        return results;

    }

    public async Task<bool> ProjectExists(int id)
    {
        return await _db.Projects.AnyAsync(p => p.IdProject == id);
    }

    public async Task<bool> UserExists(int id)
    {
        return await _db.Users.AnyAsync(u => u.IdUser == id);
    }

    public async Task<bool> UserHasAccess(int IdUser, int IdProject)
    {
        return await _db.Users.Where(u => u.IdUser == IdUser).AnyAsync(u => u.IdProjects.Any(p => p.IdProject == IdProject));
    }

    public async Task<bool> CreateTask(string name, string description, int idProject, int idReporter, int idAssignee)
    {
        Models.Task task = new Models.Task()
        {
            Name = name,
            Description = description,
            IdProject = idProject,
            IdReporter = idReporter,
            CreatedAt = DateTime.Now,
            IdAssignee = idAssignee
        };
        await _db.Tasks.AddAsync(task);
        return true;
    }

    public async Task<int> GetDefaultAssigneeIdAsync(int idProject)
    {
        Project project = await _db.Projects.Where(p => p.IdProject == idProject).FirstOrDefaultAsync();
        return project.IdDefaultAssignee;
    }

    public async Task<bool> TaskExists(int id)
    {
        return await _db.Tasks.AnyAsync(t => t.IdTask == id);
    }
}