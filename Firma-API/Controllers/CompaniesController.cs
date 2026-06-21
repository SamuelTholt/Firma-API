using Firma_API.Data;
using Firma_API.Dtos;
using Firma_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly CompanyApiDbContext _context;
    public CompaniesController(CompanyApiDbContext context)
    {
        _context = context;
    }

    // GET: api/allCompanies
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CompanyDto>), 200)]
    public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
    {
        var companies = await _context.Companies
           .Include(comp => comp.Director)
           .ToListAsync();

        return Ok(companies.Select(comp => new CompanyDto(
            comp.Id,
            comp.Name,
            comp.Code,
            comp.DirectorId,
            comp.Director != null ? $"{comp.Director.FirstName} {comp.Director.LastName}" : null
            )));
    }

    // GET: api/Company/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CompanyDetailDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<Company>> GetCompaniesById(int id)
    {
        var company = await _context.Companies
           .Include(comp => comp.Director)
           .Include(comp => comp.Divisions)
           .ThenInclude(div => div.Leader)
           .FirstOrDefaultAsync(comp => comp.Id == id);

        if (company == null)
        {
            return NotFound(new ErrorResponse($"Firma s ID {id} neexistuje!"));
        }


        return Ok(new CompanyDetailDto(
            company.Id,
            company.Name,
            company.Code,
            company.DirectorId,
            company.Director != null ? $"{company.Director.FirstName} {company.Director.LastName}" : null,
            company.Divisions.Select(div => new DivisionDto(
                div.Id,
                div.Name,
                div.Code,
                div.CompanyId,
                div.LeaderId,
                div.Leader != null ? $"{div.Leader.FirstName} {div.Leader.LastName}" : null
            ))
        ));
    }

    // PUT: api/Company/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CompanyDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyRequest req)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new ErrorResponse("Validácia zlyhala.", errors));
        }

        var comp = await _context.Companies.FindAsync(id);
        if (comp == null)
            return NotFound(new ErrorResponse($"Firma s ID {id} neexistuje."));

        if (await _context.Companies.AnyAsync(c => c.Code == req.Code && c.Id != id))
            return BadRequest(new ErrorResponse("Firma s týmto kódom už existuje."));

        if (req.DirectorId.HasValue &&
            !await _context.Employees.AnyAsync(e => e.Id == req.DirectorId))
            return BadRequest(new ErrorResponse("Zadaný riaditeľ neexistuje."));


        comp.Name = req.Name;
        comp.Code = req.Code;
        comp.DirectorId = req.DirectorId;

        await _context.SaveChangesAsync();

        return Ok(new CompanyDto(
            comp.Id,
            comp.Name,
            comp.Code,
            comp.DirectorId,
            null
        ));
    }

    // POST: api/Company
    [HttpPost]
    [ProducesResponseType(typeof(CompanyDto), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<ActionResult<Company>> CreateCompany(CreateCompanyRequest req)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new ErrorResponse("Validácia zlyhala.", errors));
        }

        if (await _context.Companies.AnyAsync(c => c.Code == req.Code))
            return BadRequest(new ErrorResponse("Firma s týmto kódom už existuje."));

        if (req.DirectorId.HasValue &&
            !await _context.Employees.AnyAsync(e => e.Id == req.DirectorId))
            return BadRequest(new ErrorResponse("Zadaný riaditeľ neexistuje."));

        var company = new Company
        {
            Name = req.Name,
            Code = req.Code,
            DirectorId = req.DirectorId
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCompaniesById), new { id = company.Id }, new CompanyDto(
            company.Id,
            company.Name,
            company.Code,
            company.DirectorId,
            null
        ));
    }

    // DELETE: api/Company/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null)
            return NotFound(new ErrorResponse($"Firma s ID {id} neexistuje."));

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
