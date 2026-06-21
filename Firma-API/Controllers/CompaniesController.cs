using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firma_API.Models;
using Firma_API.Data;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly CompanyApiDbContext _context;
    public CompaniesController(CompanyApiDbContext context)
    {
        _context = context;
    }

    // GET: api/Company
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompany()
    {
        return await _context.Companies.ToListAsync();
    }

    // GET: api/Company/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetCompany(int id)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        return company;
    }

    // PUT: api/Company/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCompany(int? id, Company company)
    {
        if (id != company.Id)
        {
            return BadRequest();
        }

        _context.Entry(company).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CompanyExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Company
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Company>> PostCompany(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCompany", new { id = company.Id }, company);
    }

    // DELETE: api/Company/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(int? id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null)
        {
            return NotFound();
        }

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CompanyExists(int? id)
    {
        return _context.Companies.Any(e => e.Id == id);
    }
}
