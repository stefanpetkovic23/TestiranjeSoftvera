using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

[ApiController]
[Route("api/[controller]")]
public class TeamMembersController : ControllerBase
{
    public TaskMenagmentDbContext _context; // Update with your actual DbContext

    public TeamMembersController(TaskMenagmentDbContext context)
    {
        _context = context;
    }

    // GET: api/TeamMembers
    [HttpGet("PreuzmiClanoveTima")]
    public async Task<ActionResult> PreuzmiClanoveTima()
    {
        return Ok(await _context.TeamMembers
        .Include(tm => tm.Team)
        .Select(tm => new
        {
            TeamMemberID = tm.Id,
            Ime = tm.FirstName,
            Prezime = tm.LastName,
            TimNaziv = tm.Team != null ? tm.Team.Name : null,
            Email = tm.Email
            // Dodajte ostale propertije prema potrebi
        })
        .ToListAsync());
    }

    // GET: api/TeamMembers/1
    [HttpGet("{id}")]
    public async Task<ActionResult<TeamMember>> GetTeamMember(int id)
    {
        var teamMember = await _context.TeamMembers.FindAsync(id);

        if (teamMember == null)
        {
            return NotFound();
        }

        return teamMember;
    }

    // POST: api/TeamMembers
    [Route("DodajClana/{firstname}/{lastname}/{email}/{teamId}/{projectId}")]
    [HttpPost]
    public async Task<ActionResult> DodajClana(string firstname, string lastname,string email,int teamId,int projectId)
    {
        TeamMember member = new TeamMember
        {
            FirstName = firstname,
            LastName = lastname,
            Email = email,
            TeamId = teamId,
            ProjectId = projectId,
        };
        try
        {
            _context.TeamMembers.Add(member);
            await _context.SaveChangesAsync();
            return Ok(member);
        }
        catch (Exception e)
        { 
            return BadRequest(e.Message);
        }
        

       
    }

    // PUT: api/TeamMembers/1
    [HttpPut("(IzmeniMembera)/{id}")]
    public async Task<IActionResult> IzmeniMembera(int id,[FromQuery] string? email,[FromQuery] int teamId)
    {
        try
    {
        var stariClanTima = await _context.TeamMembers.FindAsync(id);

        if (stariClanTima != null)
        {
            stariClanTima.Email = email ?? stariClanTima.Email;
            stariClanTima.TeamId = teamId != default(int) ? teamId : stariClanTima.TeamId;

            _context.TeamMembers.Update(stariClanTima);
            await _context.SaveChangesAsync();
            return Ok($"ID izmenjenog člana tima je: {id}");
        }
        else
        {
            return BadRequest($"Nije uspela izmena člana tima sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

    // DELETE: api/TeamMembers/1
    [HttpDelete("(ObrisiMembera)/{id}")]
    public async Task<IActionResult> ObrisiMembera(int id)
    {
             try
    {
        var teamMember = await _context.TeamMembers.FindAsync(id);

        if (teamMember != null)
        {
            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();
            return Ok($"ID obrisanog membera je: {id}");
        }
        else
        {
            return BadRequest($"Nije pronađen member sa ID: {id}");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
    }

   
}
