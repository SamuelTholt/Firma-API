using System.ComponentModel.DataAnnotations;

namespace Firma_API.Dtos
{
    public record DivisionDto
    (
        int Id,
        string Name,
        string Code,
        int CompanyId,
        int? LeaderId,
        string? LeaderFullName
    );

    public record DivisionDetailDto
    (
        int Id,
        string Name,
        string Code,
        int CompanyId,
        int? LeaderId,
        string? LeaderFullName,
        IEnumerable<ProjectDto> Projects
    );

    public record CreateDivisionRequest
    (
        [Required(ErrorMessage = "Názov je povinný!")]
        [MaxLength(200, ErrorMessage = "Názov môže mať maximálne 200 znakov!")]
        string Name,

        [Required(ErrorMessage = "Kód je povinný!")]
        [MaxLength(50, ErrorMessage = "Kód môže mať maximálne 50 znakov!")]
        string Code,

        [Required(ErrorMessage = "CompanyId je povinné!")]
        int CompanyId,

        int? LeaderId
    );

    public record UpdateDivisionRequest
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
