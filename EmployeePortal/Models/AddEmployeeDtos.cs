namespace EmployeeProtal.Models
{
    public class AddEmployeeDtos
    {
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public string? Email { get; set; }
        public decimal Salary { get; set; }
    }
}
