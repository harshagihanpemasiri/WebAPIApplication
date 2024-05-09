using EmployeeProtal.Data;
using EmployeeProtal.Models;
using EmployeeProtal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeProtal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        public readonly ApplicationDbContext _dbContext;
        public EmployeesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            var allEmp = _dbContext.WebApiEmployees.ToList();

            return Ok(allEmp);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public ActionResult GetEmployeeById(Guid id)
        {
            var emp = _dbContext.WebApiEmployees.Find(id);

            if (emp is null)
            {
                return NotFound();
            }
            return Ok(emp);
        }

        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDtos addEmployeeDtos)
        {
            var empEntity = new Employee()
            {
                Name = addEmployeeDtos.Name,
                Email = addEmployeeDtos.Email,
                Phone = addEmployeeDtos.Phone,
                Salary = addEmployeeDtos.Salary,
            };

            _dbContext.WebApiEmployees.Add(empEntity);
            _dbContext.SaveChanges();

            return Ok(empEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var emp = _dbContext.WebApiEmployees.Find(id);

            if (emp is null)
            {
                return NotFound();
            }

            emp.Name = updateEmployeeDto.Name;
            emp.Email = updateEmployeeDto.Email;
            emp.Phone = updateEmployeeDto.Phone;
            emp.Salary = updateEmployeeDto.Salary;

            _dbContext.SaveChanges();
            return Ok(emp);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var emp = _dbContext.WebApiEmployees.Find(id);

            if (emp is null)
            {
                return NotFound();
            }

            _dbContext.WebApiEmployees.Remove(emp);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
