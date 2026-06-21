namespace Firma_API.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int DivisionId { get; set; }
        public int? LeaderId { get; set; }

        public Division Division { get; set; } = null!;
        public Employee? Leader { get; set; }
        public ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}
