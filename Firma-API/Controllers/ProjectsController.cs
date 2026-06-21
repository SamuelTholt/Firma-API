using Firma_API.Data;
using Firma_API.Dtos;
using Firma_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly CompanyApiDbContext _context;
    public ProjectsController(CompanyApiDbContext context)
    {
        _context = context;
    }

    // GET: api/allProjects
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CompanyDto>), 200)]
    public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects()
    {
        var projects = await _context.Projects
            .Include(proj => proj.Leader)
            .ToArrayAsync();

        return Ok(projects.Select(proj => new ProjectDto(
            proj.Id,
            proj.Name,
            proj.Code,
            proj.DivisionId,
            proj.LeaderId,
            proj.Leader != null ? $"{proj.Leader.FirstName} {proj.Leader.LastName}" : null
            )));
    }

    // GET: api/allProjectsByDivisions
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CompanyDto>), 200)]
    public async Task<ActionResult<IEnumerable<Project>>> GetAllProjectByDivisionId([FromQuery] int? divisionId)
    {
        var query = _context.Projects
            .Include(proj => proj.Leader)
            .AsQueryable();

        if(divisionId.HasValue)
            query = query.Where(proj => proj.DivisionId == divisionId.Value);

        var projects = await query
            .OrderBy(proj => proj.Name)
            .ToArrayAsync();

        return Ok(projects.Select(proj => new ProjectDto(
            proj.Id,
            proj.Name,
            proj.Code,
            proj.DivisionId,
            proj.LeaderId,
            proj.Leader != null ? $"{proj.Leader.FirstName} {proj.Leader.LastName}" : null
            )));
    }

    // GET: api/Project/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProjectDetailDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<Project>> GetProjectById(int id)
    {
        var project = await _context.Projects
            .Include(proj => proj.Leader)
            .Include(proj => proj.Departments)
            .ThenInclude(depart => depart.Leader)
            .FirstOrDefaultAsync(proj => proj.Id == id);

        if (project == null)
        {
            return NotFound(new ErrorResponse($"Projekt s ID {id} neexistuje!"));
        }

        return Ok(new ProjectDetailDto(
            project.Id,
            project.Name,
            project.Code,
            project.DivisionId,
            project.LeaderId,
            project.Leader != null ? $"{project.Leader.FirstName} {project.Leader.LastName}" : null,
            project.Departments.Select(depart => new DepartmentDto(
                depart.Id,
                depart.Name,
                depart.Code,
                depart.ProjectId,
                depart.LeaderId,
                depart.Leader != null ? $"{depart.Leader.FirstName} {depart.Leader.LastName}" : null
            ))
        ));
    }

    // PUT: api/Project/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProjectDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> UpdateProject(int id, [FromQuery] UpdateProjectRequest req)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new ErrorResponse("Validácia zlyhala.", errors));
        }

        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return NotFound(new ErrorResponse($"Projekt s ID {id} neexistuje."));

        if (await _context.Projects.AnyAsync(c => c.Code == req.Code && c.Id != id))
            return BadRequest(new ErrorResponse("Projekt s týmto kódom už existuje."));

        if (req.LeaderId.HasValue &&
            !await _context.Employees.AnyAsync(e => e.Id == req.LeaderId))
            return BadRequest(new ErrorResponse("Zadaný vedúci neexistuje."));

        project.Name = req.Name;
        project.Code = req.Code;
        project.LeaderId = req.LeaderId;

        await _context.SaveChangesAsync();

        return Ok(new ProjectDto(
            project.Id,
            project.Name,
            project.Code,
            project.DivisionId,
            project.LeaderId,
            null
        ));
    }

    // POST: api/Project
    [HttpPost]
    [ProducesResponseType(typeof(ProjectDto), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<ActionResult<Project>> CreateProject(CreateProjectRequest req)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new ErrorResponse("Validácia zlyhala.", errors));
        }

        var division = await _context.Divisions.FindAsync(req.DivisionId);
        if (division == null)
            return NotFound(new ErrorResponse($"Divízia s ID {req.DivisionId} neexistuje."));

        if (req.LeaderId.HasValue &&
            !await _context.Employees.AnyAsync(e => e.Id == req.LeaderId))
            return BadRequest(new ErrorResponse("Zadaný vedúci neexistuje."));

        var project = new Project
        {
            Name = req.Name,
            Code = req.Code,
            DivisionId = req.DivisionId,
            LeaderId = req.LeaderId
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, new ProjectDto(
            project.Id,
            project.Name,
            project.Code,
            project.DivisionId,
            project.LeaderId,
            null
        ));
    }

    // DELETE: api/Project/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> DeleteProject(int? id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return NotFound(new ErrorResponse($"Projekt s ID {id} neexistuje."));

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
