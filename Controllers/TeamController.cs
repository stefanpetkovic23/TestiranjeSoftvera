using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    public TaskMenagmentDbContext _context; 
    

    public TeamsController(TaskMenagmentDbContext context)
    {
        _context = context;
    }

    // GET: api/Teams
    [HttpGet("VratiSveTimove")]
    public async Task<ActionResult> VratiSveTimove()
    {
        return Ok(await _context.Teams
        .Include(t => t.Members)
        .Select(t => new
        {
            TeamID = t.Id,
            TeamName = t.Name,
            Members = t.Members != null && t.Members.Any()
                ? t.Members.Select(m => new
                {
                    MemberID = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email
                    // Dodajte ostale propertije prema potrebi
                }).ToList()
                : null
        })
        .ToListAsync());

    }

    // GET: api/Teams/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Team>> GetTeam(int id)
    {
        var team = await _context.Teams.FindAsync(id);

        if (team == null)
        {
            return NotFound();
        }

        return team;
    }

    // POST: api/Teams
    [Route("DodajTeam/{name}")]
    [HttpPost]
    public async Task<ActionResult> DodajTeam(string name)
    {
         Team team = new Team
        {
            Name = name,
            
        };
        try
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return Ok(team);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        
    }

    // PUT: api/Teams/1
    [HttpPut("(IzmeniTim)/{id}/{name}")]
    public async Task<IActionResult> IzmeniTim(int id, string name)
    {
        try
    {
        var stariTim = await _context.Teams.FindAsync(id);

        if (stariTim != null)
        {
            stariTim.Name = name;

            _context.Teams.Update(stariTim);
            await _context.SaveChangesAsync();
            return Ok($"ID izmenjenog tima je: {id}");
        }
        else
        {
            return BadRequest($"Nije uspela izmena tima sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    // DELETE: api/Teams/1
    [HttpDelete("(ObrisiTim){id}")]
    public async Task<IActionResult> ObrisiTim(int id)
    {
         try
    {
        var tim = await _context.Teams.FindAsync(id);

        if (tim != null)
        {
            _context.Teams.Remove(tim);
            await _context.SaveChangesAsync();
            return Ok($"ID obrisanog tima je: {id}");
        }
        else
        {
            return BadRequest($"Nije pronađen tim sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

   
}
