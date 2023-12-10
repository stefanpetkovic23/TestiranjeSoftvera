using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models; // Update with your actual namespace

[ApiController]
[Route("api/[controller]")]
public class TaskCategoriesController : ControllerBase
{
    public TaskMenagmentDbContext _context; // Update with your actual DbContext

    public TaskCategoriesController(TaskMenagmentDbContext context)
    {
        _context = context;
    }

    // GET: api/TaskCategories
    [HttpGet("VratiSveKategorije")]
    public async Task<ActionResult> VratiSveKategorije()
    {
        return Ok(await _context.TaskCategories
        .Include(tc => tc.Tasks)
        .Select(tc => new
        {
            CategoryID = tc.Id,
            CategoryName = tc.Name,
            Tasks = tc.Tasks != null && tc.Tasks.Any()
                ? tc.Tasks.Select(task => new
                {
                    TaskID = task.Id,
                    TaskTitle = task.Title,
                    TaskDescription = task.Description,
                    TaskPriority = task.Priority,
                    TaskStatus = task.Status,
                    // Dodajte ostale propertije prema potrebi
                }).ToList()
                : null
        })
        .ToListAsync());
    }

    // GET: api/TaskCategories/1
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskCategory>> GetTaskCategory(int id)
    {
        var taskCategory = await _context.TaskCategories.FindAsync(id);

        if (taskCategory == null)
        {
            return NotFound();
        }

        return taskCategory;
    }

    // POST: api/TaskCategories
    [Route("DodajKategoriju/{name}")]
    [HttpPost]
    public async Task<ActionResult> DodajKategoriju(string name)
    {

        TaskCategory taskCategory = new TaskCategory
        {
            Name = name
        };
    try
    {
        _context.TaskCategories.Add(taskCategory);
        await _context.SaveChangesAsync();

        return Ok(taskCategory);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    // PUT: api/TaskCategories/1
    [HttpPut("(IzmeniKategoriju)/{id}/{name}")]
    public async Task<IActionResult> IzmeniKategoriju(int id, string name)
    {
        try
    {
        var staraKategorijaZadatka = await _context.TaskCategories.FindAsync(id);

        if (staraKategorijaZadatka != null)
        {
            staraKategorijaZadatka.Name = name;

            _context.TaskCategories.Update(staraKategorijaZadatka);
            await _context.SaveChangesAsync();
            return Ok($"ID izmenjene kategorije zadatka je: {id}");
        }
        else
        {
            return BadRequest($"Nije uspela izmena kategorije zadatka sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    // DELETE: api/TaskCategories/1
    [HttpDelete("(ObrisiKategoriju){id}")]
    public async Task<IActionResult> ObrisiKategoriju(int id)
    {
       try
    {
        var kategorijaZadatka = await _context.TaskCategories.FindAsync(id);

        if (kategorijaZadatka != null)
        {
            _context.TaskCategories.Remove(kategorijaZadatka);
            await _context.SaveChangesAsync();
            return Ok($"ID obrisanje kategorije zadatka je: {id}");
        }
        else
        {
            return BadRequest($"Nije pronađena kategorija zadatka sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    
}
