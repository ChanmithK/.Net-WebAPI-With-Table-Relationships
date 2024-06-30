namespace EmployeeWebAPI.Models
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public DepartmentDTO Department { get; set; }
        public List<ProjectDTO> Projects { get; set; }
    }

    public class DepartmentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

