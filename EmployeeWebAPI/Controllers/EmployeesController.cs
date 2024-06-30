
using EmployeeWebAPI.Data;
using EmployeeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using EmployeeWebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;




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

        [HttpGet]
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

        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDTO addEmployeeDto)
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

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = dbContext.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);

            if (employee is null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDTO updateEmployeeDTO)
        {
            var employee = dbContext.Employees.Find(id);

            if (employee is null)
            {
                return NotFound();
            }

            employee.Name = updateEmployeeDTO.Name;
            employee.Email = updateEmployeeDTO.Email;
            employee.Phone = updateEmployeeDTO.Phone;
            employee.Salary = updateEmployeeDTO.Salary;
            employee.DepartmentId = updateEmployeeDTO.DepartmentId;

            dbContext.SaveChanges();

            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }

            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}

