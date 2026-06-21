using System.ComponentModel.DataAnnotations;

namespace Firma_API.Dtos
{
    public record DepartmentDto
   (
       int Id,
       string Name,
       string Code,
       int ProjectId,
       int? LeaderId,
       string? LeaderFullName
   );

    public record CreateDepartmentRequest
    (
        [Required(ErrorMessage = "Názov je povinný!")]
        [MaxLength(200, ErrorMessage = "Názov môže mať maximálne 200 znakov!")]
        string Name,

        [Required(ErrorMessage = "Kód je povinný!")]
        [MaxLength(50, ErrorMessage = "Kód môže mať maximálne 50 znakov!")]
        string Code,

        [Required(ErrorMessage = "ProjectId je povinné!")]
        int ProjectId,

        int? LeaderId
    );

    public record UpdateDepartmentRequest
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
