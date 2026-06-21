using System.ComponentModel.DataAnnotations;

namespace Firma_API.Dtos
{
    public record ProjectDto
    (
        int Id,
        string Name,
        string Code,
        int DivisionId,
        int? LeaderId,
        string? LeaderFullName
    );

    public record ProjectDetailDto
    (
        int Id,
        string Name,
        string Code,
        int DivisionId,
        int? LeaderId,
        string? LeaderFullName,
        IEnumerable<DepartmentDto> Departments
    );

    public record CreateProjectRequest
    (
        [Required(ErrorMessage = "Názov je povinný!")]
        [MaxLength(200, ErrorMessage = "Názov môže mať maximálne 200 znakov!")]
        string Name,

        [Required(ErrorMessage = "Kód je povinný!")]
        [MaxLength(50, ErrorMessage = "Kód môže mať maximálne 50 znakov!")]
        string Code,

        [Required(ErrorMessage = "DivisionId je povinné!")]
        int DivisionId,

        int? LeaderId
    );

    public record UpdateProjectRequest
    (
        [Required(ErrorMessage = "Názov je povinný!")]
        [MaxLength(200, ErrorMessage = "Názov môže mať maximálne 200 znakov!")]
        string Name,

        [Required(ErrorMessage = "Kód je povinný!")]
        [MaxLength(50, ErrorMessage = "Kód môže mať maximálne 50 znakov!")]
        string Code,

        int? LeaderId
    );
}
