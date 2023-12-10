using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models; // Zamijenite sa stvarnim imenom vašeg projekta i namespace-a

[ApiController]
[Route("api/[controller]")]
public class DeadlinesController : ControllerBase
{
    public TaskMenagmentDbContext _context; 

    public DeadlinesController(TaskMenagmentDbContext context)
    {
        _context = context;
    }

    // GET: api/Deadlines
    [HttpGet("PreuzmiSveDeadline")]
    public async Task<ActionResult> PreuzmiSveDeadline()
    {
        return Ok(await _context.Deadlines
        .Include(d => d.Project)
        .Select(d => new
        {
            DeadlineID = d.Id,
            Datum = d.Date,
            Zavrsen = d.IsCompleted,
            
            Projekat = d.Project != null ? new
            {
                ProjekatID = d.Project.Id,
                ProjekatNaslov = d.Project.Title,
                // Dodajte ostale propertije prema potrebi
            } : null
            // Dodajte ostale propertije prema potrebi
        })
        .ToListAsync());
    }

    // GET: api/Deadlines/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Deadline>> GetDeadline(int id)
    {
        var deadline = await _context.Deadlines.FindAsync(id);

        if (deadline == null)
        {
            return NotFound();
        }

        return deadline;
    }

    // POST: api/Deadlines
    [Route("DodajDeadline/{date}/{iscompleted}/{projectId}")]
    [HttpPost]
    public async Task<ActionResult> DodajDeadline(DateTime date,bool iscompleted,int projectId)
    {
        Deadline deadline = new Deadline
    {
        Date = date,
        IsCompleted = iscompleted,
        ProjectId = projectId,
    };

    try
    {
        _context.Deadlines.Add(deadline);
        await _context.SaveChangesAsync();
        return Ok(deadline);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    // PUT: api/Deadlines/1
    
    [HttpPut("(IzmeniRok)/{id}/{isCompleted}")]
    public async Task<IActionResult> IzmeniRok(int id,[FromQuery] DateTime? deadline, bool isCompleted)
    {
        try
    {
        var stariRok = await _context.Deadlines.FindAsync(id);

        if (stariRok != null)
        {
            stariRok.Date = deadline ?? stariRok.Date;
            stariRok.IsCompleted = isCompleted;

            _context.Deadlines.Update(stariRok);
            await _context.SaveChangesAsync();
            return Ok($"ID izmenjenog roka je: {id}");
        }
        else
        {
            return BadRequest($"Nije uspela izmena roka sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    // DELETE: api/Deadlines/1
    [HttpDelete("(ObrisiRok)/{id}")]
    public async Task<IActionResult> ObrisiRok(int id)
    {
        try
    {
        var rok = await _context.Deadlines.FindAsync(id);

        if (rok != null)
        {
            _context.Deadlines.Remove(rok);
            await _context.SaveChangesAsync();
            return Ok($"ID obrisanog roka je: {id}");
        }
        else
        {
            return BadRequest($"Nije pronađen rok sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    
}
