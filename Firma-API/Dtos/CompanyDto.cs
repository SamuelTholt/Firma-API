using System.ComponentModel.DataAnnotations;

namespace Firma_API.Dtos
{
    public record CompanyDto
        (
            int Id,
            string Name,
            string Code,
            int? DirectorId,
            string? DirectorFullName
        );

    public record CompanyDetailDto
        (
            int Id,
            string Name,
            string Code,
            int? DirectorId,
            string? DirectorFullName,
            IEnumerable<DivisionDto> Divisions
        );

    public record CreateCompanyRequest
        (
            [Required(ErrorMessage = "Názov je povinný!")]
            [MaxLength(200, ErrorMessage = "Názov môže mať maximálne 200 znakov!")]
            string Name,

            [Required(ErrorMessage = "Kód je povinný!")]
            [MaxLength(50, ErrorMessage = "Kód môže mať maximálne 50 znakov!")]
            string Code,

            int? DirectorId
        );

    public record UpdateCompanyRequest
        (
            [Required(ErrorMessage = "Názov je povinný!")]
            [MaxLength(200, ErrorMessage = "Názov môže mať maximálne 200 znakov!")]
            string Name,

            [Required(ErrorMessage = "Kód je povinný!")]
            [MaxLength(50, ErrorMessage = "Kód môže mať maximálne 50 znakov!")]
            string Code,

            int? DirectorId
        );
}
