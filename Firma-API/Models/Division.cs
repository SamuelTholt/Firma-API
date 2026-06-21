using Microsoft.CodeAnalysis;

namespace Firma_API.Models
{
    public class Division
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int CompanyId { get; set; }
        public int? LeaderId { get; set; }

        public Company Company { get; set; } = null!;
        public Employee? Leader { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
