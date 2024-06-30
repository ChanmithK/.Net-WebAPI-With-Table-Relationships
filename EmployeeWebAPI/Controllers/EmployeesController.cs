
using EmployeeWebAPI.Data;
using EmployeeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using EmployeeWebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace EmployeeWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //********************* Get All Employees *********************
        [HttpGet, Authorize]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = dbContext.Employees
                .Include(e => e.Department)
                .Include(e => e.EmployeeProjects).ThenInclude(ep => ep.Project)
                .ToList();

            // Map entities to DTOs
            var employeeDTOs = allEmployees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Phone = e.Phone,
                Salary = e.Salary,
                Department = new DepartmentDTO
                {
                    Id = e.Department.Id,
                    Name = e.Department.Name
                },
                Projects = e.EmployeeProjects.Select(ep => new ProjectDTO
                {
                    Id = ep.Project.Id,
                    Name = ep.Project.Name
                }).ToList()
            }).ToList();

            return Ok(employeeDTOs);
        }

        //********************* Add New Employee *********************
        [HttpPost]
        public IActionResult AddEmployee([FromBody] AddEmployeeDTO addEmployeeDto)
        {
            var employeeEntity = new Employee()
            {
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary,
                DepartmentId = addEmployeeDto.DepartmentId

            };

            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();

            return Ok(employeeEntity);
        }


        //********************* Get Employee By ID *********************
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = dbContext.Employees
                .Include(e => e.Department)
                .Include(e => e.EmployeeProjects).ThenInclude(ep => ep.Project)
                .FirstOrDefault(e => e.Id == id);

            if (employee is null)
            {
                return NotFound();
            }

            // Mapping to DTO
            var employeeDTO = new EmployeeDTO
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                Salary = employee.Salary,
                Department = new DepartmentDTO
                {
                    Id = employee.Department.Id,
                    Name = employee.Department.Name
                },
                Projects = employee.EmployeeProjects.Select(ep => new ProjectDTO
                {
                    Id = ep.Project.Id,
                    Name = ep.Project.Name
                }).ToList()
            };

            return Ok(employeeDTO);
        }


        //********************* Update Employee *********************
        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, [FromBody] UpdateEmployeeDTO updateEmployeeDTO) //We use this [FromBody] to Binding Data from the Request Body
        {
            var employeeToUpdate = dbContext.Employees
                .Include(e => e.EmployeeProjects)
                .Include(e => e.Department)
                .FirstOrDefault(e => e.Id == id);

            if (employeeToUpdate == null)
            {
                return NotFound();
            }

            // Update scalar properties
            employeeToUpdate.Name = updateEmployeeDTO.Name;
            employeeToUpdate.Email = updateEmployeeDTO.Email;
            employeeToUpdate.Phone = updateEmployeeDTO.Phone;
            employeeToUpdate.Salary = updateEmployeeDTO.Salary;

            // Update Department if DepartmentId is provided
            if (updateEmployeeDTO.DepartmentId.HasValue)
            {
                var department = dbContext.Departments.Find(updateEmployeeDTO.DepartmentId.Value);
                if (department == null)
                {
                    return BadRequest("Invalid department Id");
                }
                employeeToUpdate.DepartmentId = updateEmployeeDTO.DepartmentId.Value;
            }

            // Update Projects
            if (updateEmployeeDTO.Projects != null && updateEmployeeDTO.Projects.Any())
            {
                // Clear existing associations
                dbContext.EmployeeProjects.RemoveRange(employeeToUpdate.EmployeeProjects);

                // Add new associations
                foreach (var projectDTO in updateEmployeeDTO.Projects)
                {
                    var projectExists = dbContext.Projects.Any(p => p.Id == projectDTO.ProjectId);
                    if (!projectExists)
                    {
                        return BadRequest($"Invalid project Id: {projectDTO.ProjectId}");
                    }

                    employeeToUpdate.EmployeeProjects.Add(new EmployeeProject
                    {
                        EmployeeId = id,
                        ProjectId = projectDTO.ProjectId
                    });
                }
            }

            dbContext.SaveChanges();

            return NoContent();
        }


        //********************* Delete Employee *********************
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employeeToDelete = dbContext.Employees
                .Include(e => e.EmployeeProjects) // Include if you need to manage associations
                .FirstOrDefault(e => e.Id == id);

            if (employeeToDelete == null)
            {
                return NotFound();
            }

            // Optionally, manage associations if needed
            dbContext.EmployeeProjects.RemoveRange(employeeToDelete.EmployeeProjects);

            dbContext.Employees.Remove(employeeToDelete);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}

