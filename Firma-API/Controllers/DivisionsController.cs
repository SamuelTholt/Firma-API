using Firma_API.Data;
using Firma_API.Dtos;
using Firma_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class DivisionsController : ControllerBase
{
    private readonly CompanyApiDbContext _context;
    public DivisionsController(CompanyApiDbContext context)
    {
        _context = context;
    }

    // GET: api/Divisions
    // GET: api/Divisions?companyId=1
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DivisionDto>), 200)]
    public async Task<ActionResult<IEnumerable<Division>>> GetAllDivisions([FromQuery] int? companyId)
    {
        var query = _context.Divisions
            .Include(div => div.Leader)
            .AsQueryable();

        if (companyId.HasValue)
            query = query.Where(div => div.CompanyId == companyId.Value);

        var divisions = await query
            .OrderBy(div => div.Name)
            .ToListAsync();

        return Ok(divisions.Select(div => new DivisionDto(
            div.Id,
            div.Name,
            div.Code,
            div.CompanyId,
            div.LeaderId,
            div.Leader != null ? $"{div.Leader.FirstName} {div.Leader.LastName}" : null
        )));
    }

    // GET: api/Division/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CompanyDetailDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<Division>> GetDivisionById(int id)
    {
        var division = await _context.Divisions
            .Include(div => div.Leader)
            .Include(div => div.Projects)
            .ThenInclude(proj => proj.Leader)
            .FirstOrDefaultAsync(div => div.Id == id);

        if (division == null)
        {
            return NotFound(new ErrorResponse($"Divízia s ID {id} neexistuje!"));
        }

        return Ok(new DivisionDetailDto(
            division.Id,
            division.Name,
            division.Code,
            division.CompanyId,
            division.LeaderId,
            division.Leader != null ? $"{division.Leader.FirstName} {division.Leader.LastName}" : null,
            division.Projects.Select(proj => new ProjectDto(
                proj.Id,
                proj.Name,
                proj.Code,
                proj.DivisionId,
                proj.LeaderId,
                proj.Leader != null ? $"{proj.Leader.FirstName} {proj.Leader.LastName}" : null
                ))
        ));
    }

    // PUT: api/Division/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(DivisionDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> UpdateDivision(int id, [FromBody] UpdateDivisionRequest req)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new ErrorResponse("Validácia zlyhala.", errors));
        }

        var division = await _context.Divisions.FindAsync(id);
        if (division == null)
            return NotFound(new ErrorResponse($"Divízia s ID {id} neexistuje."));

        if (await _context.Divisions.AnyAsync(c => c.Code == req.Code && c.Id != id))
            return BadRequest(new ErrorResponse("Divízia s týmto kódom už existuje."));

        if (req.LeaderId.HasValue &&
            !await _context.Employees.AnyAsync(e => e.Id == req.LeaderId))
            return BadRequest(new ErrorResponse("Zadaný vedúci neexistuje."));

        division.Name = req.Name;
        division.Code = req.Code;
        division.LeaderId = req.LeaderId;

        await _context.SaveChangesAsync();

        return Ok(new DivisionDto(
            division.Id,
            division.Name,
            division.Code,
            division.CompanyId,
            division.LeaderId,
            null
        ));
    }

    // POST: api/Division
    [HttpPost]
    [ProducesResponseType(typeof(DivisionDto), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<ActionResult<Division>> CreateDivision(CreateDivisionRequest req)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new ErrorResponse("Validácia zlyhala.", errors));
        }

        var company = await _context.Companies.FindAsync(req.CompanyId);
        if (company == null)
            return NotFound(new ErrorResponse($"Firma s ID {req.CompanyId} neexistuje."));

        if (req.LeaderId.HasValue &&
            !await _context.Employees.AnyAsync(e => e.Id == req.LeaderId))
            return BadRequest(new ErrorResponse("Zadaný vedúci neexistuje."));

        var division = new Division
        {
            Name = req.Name,
            Code = req.Code,
            CompanyId = req.CompanyId,
            LeaderId = req.LeaderId
        };

        _context.Divisions.Add(division);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDivisionById), new { id = division.Id }, new DivisionDto(
            division.Id,
            division.Name,
            division.Code,
            division.CompanyId,
            division.LeaderId,
            null
        ));
    }

    // DELETE: api/Division/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]

    public async Task<IActionResult> DeleteDivision(int? id)
    {
        var division = await _context.Divisions.FindAsync(id);
        if (division == null)
            return NotFound(new ErrorResponse($"Divízia s ID {id} neexistuje."));

        _context.Divisions.Remove(division);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
