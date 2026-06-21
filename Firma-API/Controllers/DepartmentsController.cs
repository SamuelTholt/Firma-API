using Firma_API.Data;
using Firma_API.Dtos;
using Firma_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly CompanyApiDbContext _context;
    public DepartmentsController(CompanyApiDbContext context)
    {
        _context = context;
    }

    // GET: api/Departments
    // GET: api/Departments?projectId=1
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), 200)]
    public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments([FromQuery] int? projectId)
    {
        var query = _context.Departments
            .Include(dep => dep.Leader)
            .AsQueryable();

        if (projectId.HasValue)
            query = query.Where(dep => dep.ProjectId == projectId.Value);

        var depart = await query
            .OrderBy(dep => dep.Name)
            .ToListAsync();

        return Ok(depart.Select(dep => new DepartmentDto(
            dep.Id,
            dep.Name,
            dep.Code,
            dep.ProjectId,
            dep.LeaderId,
            dep.Leader != null ? $"{dep.Leader.FirstName} {dep.Leader.LastName}" : null
        )));
    }

    // GET: api/Department/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DepartmentDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<Department>> GetDepartmentById(int id)
    {
        var depart = await _context.Departments
            .Include(div => div.Leader)
            .FirstOrDefaultAsync(div => div.Id == id);

        if (depart == null)
        {
            return NotFound(new ErrorResponse($"Oddelenie s ID {id} neexistuje!"));
        }

        return Ok(new DepartmentDto(
            depart.Id,
            depart.Name,
            depart.Code,
            depart.ProjectId,
            depart.LeaderId,
            depart.Leader != null ? $"{depart.Leader.FirstName} {depart.Leader.LastName}" : null
        ));
    }

    // PUT: api/Department/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(DepartmentDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentRequest req)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new ErrorResponse("Validácia zlyhala.", errors));
        }

        var depart = await _context.Departments.FindAsync(id);
        if (depart == null)
            return NotFound(new ErrorResponse($"Oddelenie s ID {id} neexistuje."));

        if (await _context.Departments.AnyAsync(c => c.Code == req.Code && c.Id != id))
            return BadRequest(new ErrorResponse("Oddelenie s týmto kódom už existuje."));

        if (req.LeaderId.HasValue &&
            !await _context.Employees.AnyAsync(e => e.Id == req.LeaderId))
            return BadRequest(new ErrorResponse("Zadaný vedúci neexistuje."));

        depart.Name = req.Name;
        depart.Code = req.Code;
        depart.LeaderId = req.LeaderId;

        await _context.SaveChangesAsync();

        return Ok(new DepartmentDto(
            depart.Id,
            depart.Name,
            depart.Code,
            depart.ProjectId,
            depart.LeaderId,
            null
        ));
    }

    // POST: api/Department
    [HttpPost]
    [ProducesResponseType(typeof(DepartmentDto), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<ActionResult<Department>> CreateDepartment(CreateDepartmentRequest req)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new ErrorResponse("Validácia zlyhala.", errors));
        }

        var proj = await _context.Projects.FindAsync(req.ProjectId);
        if (proj == null)
            return NotFound(new ErrorResponse($"Firma s ID {req.ProjectId} neexistuje."));

        if (req.LeaderId.HasValue &&
            !await _context.Employees.AnyAsync(e => e.Id == req.LeaderId))
            return BadRequest(new ErrorResponse("Zadaný vedúci neexistuje."));

        var depart = new Department
        {
            Name = req.Name,
            Code = req.Code,
            ProjectId = req.ProjectId,
            LeaderId = req.LeaderId
        };

        _context.Departments.Add(depart);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDepartmentById), new { id = depart.Id }, new DepartmentDto(
            depart.Id,
            depart.Name,
            depart.Code,
            depart.ProjectId,
            depart.LeaderId,
            null
        ));
    }

    // DELETE: api/Department/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> DeleteDepartment(int? id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
            return NotFound(new ErrorResponse($"Oddelenie s ID {id} neexistuje."));

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
