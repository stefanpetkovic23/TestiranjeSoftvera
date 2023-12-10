using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models; 

[ApiController]
[Route("api/[controller]")]
public class ProjectTasksController : ControllerBase
{
    public TaskMenagmentDbContext _context; // Prilagodite sa vašim DbContext-om

    public ProjectTasksController(TaskMenagmentDbContext context)
    {
        _context = context;
    }

    // GET: api/ProjectTasks
    [HttpGet("VratiSveTaskove")]
    public async Task<ActionResult> VratiSveTaskove()
    {
        return Ok(await _context.Tasks
        .Include(pt => pt.Project)
        .Include(pt => pt.TaskCategory)
        .Select(pt => new
        {
            TaskID = pt.Id,
            TaskTitle = pt.Title,
            TaskDescription = pt.Description,
            Priority = pt.Priority,
            Status = pt.Status,
            DeadlineDays = pt.Deadline,
            Project = pt.Project != null
                ? new
                {
                    ProjectID = pt.Project.Id,
                    ProjectTitle = pt.Project.Title,
                    ProjectDescription = pt.Project.Description,
                    // Dodajte ostale propertije prema potrebi
                }
                : null,
            TaskCategory = pt.TaskCategory != null
                ? new
                {
                    CategoryID = pt.TaskCategory.Id,
                    CategoryName = pt.TaskCategory.Name
                    // Dodajte ostale propertije prema potrebi
                }
                : null
        })
        .ToListAsync());
    }

[HttpGet("VratiFiltriraneTaskove/{kategorijaId}")]
public async Task<ActionResult> VratiFiltriraneTaskove(int kategorijaId)
{
    try
    {
        var query = _context.Tasks
            .Include(pt => pt.Project)
            .Include(pt => pt.TaskCategory)
            .Where(pt =>  pt.TaskCategoryId == kategorijaId )
            .Select(pt => new
            {
                TaskID = pt.Id,
                TaskTitle = pt.Title,
                TaskDescription = pt.Description,
                Priority = pt.Priority,
                Status = pt.Status,
                DeadlineDays = pt.Deadline,
                Project = pt.Project != null
                    ? new
                    {
                        ProjectID = pt.Project.Id,
                        ProjectTitle = pt.Project.Title,
                        ProjectDescription = pt.Project.Description,
                        // Dodajte ostale propertije prema potrebi
                    }
                    : null,
                TaskCategory = pt.TaskCategory != null
                    ? new
                    {
                        CategoryID = pt.TaskCategory.Id,
                        CategoryName = pt.TaskCategory.Name
                        // Dodajte ostale propertije prema potrebi
                    }
                    : null
            });

        return Ok(await query.ToListAsync());
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    // POST: api/ProjectTasks
    [Route("DodajTask/{title}/{description}/{priority}/{status}/{deadline}/{projectId}/{taskCategoryId}/{teamMemberId}")]
    [HttpPost]
    public async Task<ActionResult> DodajTask(string title, string description, int priority, string status,int deadline, int projectId, int taskCategoryId, int teamMemberId)
    {
        ProjectTask task = new ProjectTask
        {
            Title = title,
            Description = description,
            Priority = priority,
            Status = status,
            Deadline = deadline,
            ProjectId = projectId,
            TaskCategoryId = taskCategoryId,
            TeamMemberId = teamMemberId
        };

        try
        {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return Ok(task);
        }
        catch (Exception e)
        {
         

        // Vrati informacije o grešci
        return BadRequest(e.Message);
        }
    }

    // PUT: api/ProjectTasks/1
    [HttpPut("(IzmeniTask)/{taskId}/{status}/{deadline}")]
    public async Task<IActionResult> IzmeniTask( int taskId,string status,int deadline)
    {
       try
    {
        var stariZadatak = await _context.Tasks.FindAsync(taskId);

        if (stariZadatak != null)
        {
            
            stariZadatak.Status = status;
            stariZadatak.Deadline = deadline;
            

            _context.Tasks.Update(stariZadatak);
            await _context.SaveChangesAsync();
            return Ok($"ID izmenjenog zadatka je: {taskId}");
        }
        else
        {
            return BadRequest($"Nije uspela izmena zadatka sa ID: {taskId}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    // DELETE: api/ProjectTasks/1
    [HttpDelete("(ObrisiTask)/{id}")]
    public async Task<IActionResult> ObrisiTask(int id)
    {
         try
    {
        var zadatak = await _context.Tasks.FindAsync(id);

        if (zadatak != null)
        {
            _context.Tasks.Remove(zadatak);
            await _context.SaveChangesAsync();
            return Ok($"ID obrisanog zadatka je: {id}");
        }
        else
        {
            return BadRequest($"Nije pronađen zadatak sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    
}
