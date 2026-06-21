namespace Firma_API.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public int? DirectorId { get; set; }
        public Employee? Director { get; set; }
        public ICollection<Division> Divisions { get; set; } = new List<Division>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
