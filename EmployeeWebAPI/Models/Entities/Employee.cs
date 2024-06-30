namespace EmployeeWebAPI.Models.Entities;
    public class Employee
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public decimal Salary { get; set; }
        public Guid DepartmentId { get; set; }  // Foreign Key

        // Navigation Property
        public Department Department { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();

    }

