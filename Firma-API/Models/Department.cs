namespace Firma_API.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int ProjectId { get; set; }
        public int? LeaderId { get; set; }

        public Project Project { get; set; } = null!;
        public Employee? Leader { get; set; }
    }
}
