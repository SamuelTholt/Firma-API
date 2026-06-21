using Firma_API.Data;
using Firma_API.Dtos;
using Firma_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly CompanyApiDbContext _context;
    public EmployeesController(CompanyApiDbContext context)
    {
        _context = context;
    }

    // GET: api/Employees
    // GET: api/Employees?companyId=1
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), 200)]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees([FromQuery] int? companyId)
    {
        var query = _context.Employees.AsQueryable();

        if (companyId.HasValue)
        {
            query = query.Where(emp => emp.CompanyId == companyId.Value);
        }

        var employees = await query
            .OrderBy(emp => emp.LastName)
            .ThenBy(emp => emp.FirstName)
            .ToListAsync();

        return Ok(employees.Select(emp => new EmployeeDto(
            emp.Id,
            emp.Title,
            emp.FirstName,
            emp.LastName,
            emp.Phone,
            emp.Email,
            emp.CompanyId
            )));
    }

    // GET: api/Employee/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmployeeDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var emp = await _context.Employees.FindAsync(id);
        if (emp == null)
        {
            return NotFound(new ErrorResponse($"Zamestnanec s ID {id} neexistuje."));
        }

        return Ok(new EmployeeDto(
            emp.Id,
            emp.Title,
            emp.FirstName,
            emp.LastName,
            emp.Phone,
            emp.Email,
            emp.CompanyId
        ));
    }

    // PUT: api/Employee/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EmployeeDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeRequest req)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new ErrorResponse("Validácia zlyhala.", errors));
        }

        var emp = await _context.Employees.FindAsync(id);
        if (emp == null)
            return NotFound(new ErrorResponse($"Zamestnanec s ID {id} neexistuje."));

        emp.Title = req.Title;
        emp.FirstName = req.FirstName;
        emp.LastName = req.LastName;
        emp.Phone = req.Phone;
        emp.Email = req.Email;

        await _context.SaveChangesAsync();

        return Ok(new EmployeeDto(
            emp.Id,
            emp.Title,
            emp.FirstName,
            emp.LastName,
            emp.Phone,
            emp.Email,
            emp.CompanyId
        ));
    }

    // POST: api/Employee
    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee([FromBody] CreateEmployeeRequest req)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new ErrorResponse("Validácia zlyhala.", errors));
        }

        var company = await _context.Companies.FindAsync(req.CompanyId);
        if (company == null)
            return NotFound(new ErrorResponse($"Firma s ID {req.CompanyId} neexistuje."));

        var emp = new Employee
        {
            Title = req.Title,
            FirstName = req.FirstName,
            LastName = req.LastName,
            Phone = req.Phone,
            Email = req.Email,
            CompanyId = req.CompanyId
        };

        _context.Employees.Add(emp);
        await _context.SaveChangesAsync();

        var dto = new EmployeeDto(
            emp.Id,
            emp.Title,
            emp.FirstName,
            emp.LastName,
            emp.Phone,
            emp.Email,
            emp.CompanyId
        );

        return CreatedAtAction(nameof(GetById), new { id = emp.Id }, dto);
    }

    // DELETE: api/Employee/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> DeleteEmployee(int? id)
    {
        var emp = await _context.Employees.FindAsync(id);
        if (emp == null)
            return NotFound(new ErrorResponse($"Zamestnanec s ID {id} neexistuje."));

        _context.Employees.Remove(emp);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
