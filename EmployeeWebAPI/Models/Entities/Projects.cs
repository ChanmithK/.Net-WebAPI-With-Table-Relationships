namespace EmployeeWebAPI.Models.Entities;

public class Project
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
}