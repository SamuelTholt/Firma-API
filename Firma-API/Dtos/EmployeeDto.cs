using System.ComponentModel.DataAnnotations;

namespace Firma_API.Dtos
{
    public record EmployeeDto
        (
            int Id,
            string? Title,
            string FirstName,
            string LastName,
            string Phone,
            string Email,
            int CompanyId
        );

    public record CreateEmployeeRequest
        (
            [MaxLength(50, ErrorMessage = "Titul môže mať maximálne 50 znakov!")]
            string? Title,

            [Required(ErrorMessage = "Meno je povinné!")]
            [MaxLength(100, ErrorMessage = "Meno môže mať maximálne 100 znakov!")]
            string FirstName,

            [Required(ErrorMessage = "Priezvisko je povinné!")]
            [MaxLength(100, ErrorMessage = "Priezvisko môže mať maximálne 100 znakov!")]
            string LastName,

            [Required(ErrorMessage = "Tel. číslo je povinné!")]
            [MaxLength(30, ErrorMessage = "Tel. číslo môže mať maximálne 30 znakov!")]
            string Phone,

            [Required(ErrorMessage = "Email je povinný!")]
            [EmailAddress(ErrorMessage = "Email nemá správny formát.")]
            [MaxLength(200, ErrorMessage = "Email môže mať maximálne 200 znakov!")]
            string Email,

            [Required(ErrorMessage = "CompanyId je povinné.")]
            int CompanyId
        );

    public record UpdateEmployeeRequest
        (
            [MaxLength(50, ErrorMessage = "Titul môže mať maximálne 50 znakov!")]
            string? Title,

            [Required(ErrorMessage = "Meno je povinné!")]
            [MaxLength(100, ErrorMessage = "Meno môže mať maximálne 100 znakov!")]
            string FirstName,

            [Required(ErrorMessage = "Priezvisko je povinné!")]
            [MaxLength(100, ErrorMessage = "Priezvisko môže mať maximálne 100 znakov!")]
            string LastName,

            [Required(ErrorMessage = "Tel. číslo je povinné!")]
            [MaxLength(30, ErrorMessage = "Tel. číslo môže mať maximálne 30 znakov!")]
            string Phone,

            [Required(ErrorMessage = "Email je povinný!")]
            [EmailAddress(ErrorMessage = "Email nemá správny formát.")]
            [MaxLength(200, ErrorMessage = "Email môže mať maximálne 200 znakov!")]
            string Email
        );
}
