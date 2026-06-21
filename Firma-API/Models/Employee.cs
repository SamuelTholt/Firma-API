namespace Firma_API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;
    }
}
