using DepEmpCardAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DepEmpCardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(ApplicationContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Employee
        [HttpGet]
        public IQueryable<EmployeeDTO> GetEmployees()
        {
            var employees = from e in _context.Employees
                        select new EmployeeDTO()
                        {
                            EmployeeId = e.EmployeeId,
                            EmployeeName = e.EmployeeName,
                            EmployeeSurname = e.EmployeeSurname,
                            DepartmentName  = e.DepartmentName,
                            DateOfJoining = e.DateOfJoining,
                            PhotoFileName = e.PhotoFileName
                        };

            return employees;
        }

        // GET api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employees.Select(e =>
              new EmployeeDTO()
              {
                  EmployeeId = e.EmployeeId,
                  EmployeeName = e.EmployeeName,
                  EmployeeSurname = e.EmployeeSurname,
                  DepartmentName = e.DepartmentName,
                  DateOfJoining = e.DateOfJoining,
                  PhotoFileName = e.PhotoFileName
              }
            ).SingleOrDefaultAsync(b => b.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }


        // POST api/Employee
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            var dep = _context.Departments.FirstOrDefault(p => p.DepartmentName == employee.DepartmentName);

            if (dep != null)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
            }
            else
                return BadRequest("Such a department does not exist");
        }

        // PUT api/Employee
        [HttpPut]
        public async Task<IActionResult> PutEmployee(Employee employee)
        {
            var dep = _context.Departments.FirstOrDefault(p => p.DepartmentName == employee.DepartmentName);

            _context.Entry(employee).State = EntityState.Modified;

            if (dep != null)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else return BadRequest("Such a department does not exist");

            return new JsonResult("Updated Successfully");
        }

        // DELETE api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }


        [Route("SaveFile")]
        [HttpPost]
        public async Task<IActionResult> SaveFile(FileMode fileMode)
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photo/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }


        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public JsonResult GetGetAllDepartmentNames()
        {
            var names = _context.Departments.Select(p => p.DepartmentName).ToArray();

            return new JsonResult(names);
        }

    }
}
