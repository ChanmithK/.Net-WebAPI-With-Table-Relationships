namespace EmployeeWebAPI.Models
{
    public class UpdateEmployeeDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public Guid? DepartmentId { get; set; }
        public List<UpdateEmployeeProjectDTO> Projects { get; set; }
    }
}

