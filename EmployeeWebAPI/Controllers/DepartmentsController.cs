
using EmployeeWebAPI.Data;
using EmployeeWebAPI.Models;
using EmployeeWebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public DepartmentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            var allDepartments = dbContext.Departments.ToList();
            return Ok(allDepartments);
        }

        [HttpPost]
        public IActionResult AddDepartment(AddDepartmentDTO addDepartmentDto)
        {
            var departmentEntity = new Department()
            {
                Name = addDepartmentDto.Name
            };

            dbContext.Departments.Add(departmentEntity);
            dbContext.SaveChanges();

            return Ok(departmentEntity);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetDepartmentById(Guid id)
        {
            var department = dbContext.Departments.Find(id);

            if (department is null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateDepartment(Guid id, UpdateDepartmentDTO updateDepartmentDTO)
        {
            var department = dbContext.Departments.Find(id);

            if (department is null)
            {
                return NotFound();
            }
            department.Name = updateDepartmentDTO.Name;

            dbContext.SaveChanges();

            return Ok(department);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteDepartment(Guid id)
        {
            var department = dbContext.Departments.Find(id);
            if (department is null)
            {
                return NotFound();
            }

            dbContext.Departments.Remove(department);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}

