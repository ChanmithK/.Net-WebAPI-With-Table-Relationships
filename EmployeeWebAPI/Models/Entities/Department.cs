namespace EmployeeWebAPI.Models.Entities;

public class Department
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    //Navigation Property
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

