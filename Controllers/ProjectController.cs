using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models; // Update with your actual namespace

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    public TaskMenagmentDbContext _context; // Update with your actual DbContext

    public ProjectsController(TaskMenagmentDbContext context)
    {
        _context = context;
    }

    // GET: api/Projects
    [HttpGet("VratiSveProjekte")]
    public async Task<ActionResult> VratiSveProjekte()
    {
        return Ok(await _context.Projects
        .Include(p => p.TeamMembers)
        .Include(p => p.Tasks)
        .Select(p => new
        {
            ProjectID = p.Id,
            ProjectTitle = p.Title,
            ProjectDescription = p.Description,
            TeamMembers = p.TeamMembers != null && p.TeamMembers.Any()
                ? p.TeamMembers.Select(tm => new
                {
                    TeamMemberID = tm.Id,
                    FirstName = tm.FirstName,
                    LastName = tm.LastName,
                    Email = tm.Email
                    // Dodajte ostale propertije prema potrebi
                }).ToList()
                : null,
            Tasks = p.Tasks != null && p.Tasks.Any()
                ? p.Tasks.Select(task => new
                {
                    TaskID = task.Id,
                    TaskTitle = task.Title,
                    // Dodajte ostale propertije prema potrebi
                }).ToList()
                : null,
            
        })
        .ToListAsync());
    }

    // GET: api/Projects/1
  [HttpGet("VratiFiltriraneProjekte")]
public async Task<ActionResult> VratiFiltriraneProjekte([FromQuery] int? brojDanaZaFiltriranje)
{
    try
    {
        var query = _context.Projects
            .Include(p => p.TeamMembers)
            .Include(p => p.Tasks)
            .Select(p => new
            {
                ProjectID = p.Id,
                ProjectTitle = p.Title,
                ProjectDescription = p.Description,
                Deadline = p.Deadline != null ? p.Deadline.Date : (DateTime?)null, // Dodato Deadline
                TeamMembers = p.TeamMembers != null && p.TeamMembers.Any()
                    ? p.TeamMembers.Select(tm => new
                    {
                        TeamMemberID = tm.Id,
                        FirstName = tm.FirstName,
                        LastName = tm.LastName,
                        Email = tm.Email
                        // Dodajte ostale propertije prema potrebi
                    }).ToList()
                    : null,
                Tasks = p.Tasks != null && p.Tasks.Any()
                    ? p.Tasks.Select(task => new
                    {
                        TaskID = task.Id,
                        TaskTitle = task.Title,
                        // Dodajte ostale propertije prema potrebi
                    }).ToList()
                    : null,
            });

        // Ako je zadat brojDanaZaFiltriranje, primeni filtriranje prema broju dana
        if (brojDanaZaFiltriranje.HasValue)
        {
            DateTime datumPriblizavanja = DateTime.Now.AddDays(brojDanaZaFiltriranje.Value);
            query = query.Where(p => p.Deadline != null && p.Deadline.Value.Date <= datumPriblizavanja);
        }

        return Ok(await query.ToListAsync());
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}



    // POST: api/Projects
    [Route("DodajProjekat/{title}/{description}")]
    [HttpPost]
    public async Task<ActionResult> DodajProjekat(string title,string description)
    {
        Project project = new Project
        {
            Title = title,
            Description = description
        };

         try{
        

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return Ok(project);
        }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    // PUT: api/Projects/1
    [HttpPut("(IzmeniProjekat)/{id}/{description}")]
    public async Task<IActionResult> IzmeniProjekat(int id,[FromQuery] string? title, string description)
    {
        try
    {
        var stariProjekat = await _context.Projects.FindAsync(id);

        if (stariProjekat != null)
        {
            stariProjekat.Title = title ?? stariProjekat.Title;
            stariProjekat.Description = description;

            _context.Projects.Update(stariProjekat);
            await _context.SaveChangesAsync();
            return Ok($"ID izmenjenog projekta je: {id}");
        }
        else
        {
            return BadRequest($"Nije uspela izmena projekta sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    // DELETE: api/Projects/1
    [HttpDelete("(ObrisiProjekat){id}")]
    public async Task<IActionResult> ObrisiProjekat(int id)
    {
        try
    {
        var projekat = await _context.Projects.FindAsync(id);

        if (projekat != null)
        {
            _context.Projects.Remove(projekat);
            await _context.SaveChangesAsync();
            return Ok($"ID obrisanog projekta je: {id}");
        }
        else
        {
            return BadRequest($"Nije pronađen projekat sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    
}
