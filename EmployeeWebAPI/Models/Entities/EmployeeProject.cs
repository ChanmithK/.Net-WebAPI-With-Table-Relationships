namespace EmployeeWebAPI.Models.Entities;

//We use to this EmployeeProject table for join the Employee and Project table.
//because Entity Framework Core (EF Core) does not directly support many-to-many relationships without an explicit join entity.
public class EmployeeProject
{
    public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }

    // Navigation Property
    public Employee Employee { get; set; }
    public Project Project { get; set; }
}

