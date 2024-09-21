using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        private EmployeeResponse printEmployee(Employee emp)
        {
            var employeeModel = new EmployeeResponse()
            {
                Id = emp.Id,
                Email = emp.Email,
                Roles = emp.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = emp.FullName,
                AppliedPromocodesCount = emp.AppliedPromocodesCount
            };
            return employeeModel;
        }


        /// <summary>
        /// Добавить сотрудника
        /// </summary>
        /// <param name="employee"></param>
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployee(Employee employee)
        {
            if (await _employeeRepository.GetByIdAsync(employee.Id) != null)
                return NotFound("Employee with this ID exist");
            else
            {
                var resEmp = await _employeeRepository.Create(employee);
                if (resEmp == null)
                    return NotFound();
                return printEmployee(resEmp);
            }
        }

        /// <summary>
        /// Удалить сотрудника по id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task<ActionResult<EmployeeResponse>> RemoveEmployee(Guid id)
        {
            if (await _employeeRepository.GetByIdAsync(id) != null)
            {
                var resEmp = await _employeeRepository.Remove(id);
                if (resEmp == null)
                    return Ok("Employee was removed");
                else
                    return BadRequest();
            }
            else return NotFound("Employee not found");
        }

        /// <summary>
        /// Обновить данные о сотруднике с указанным id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        [HttpPatch]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployee(Guid id, Employee employee)
        {
            var existingEmp = await _employeeRepository.GetByIdAsync(id);
            if (existingEmp == null)
                return NotFound("Employee with this ID not found");
            existingEmp.FirstName = employee.FirstName;
            existingEmp.LastName = employee.LastName;
            existingEmp.Email = employee.Email;
            existingEmp.Roles = employee.Roles.Select(r => new Role {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }).ToList();
            existingEmp.AppliedPromocodesCount = employee.AppliedPromocodesCount;

            var resEmp = await _employeeRepository.Update(id, employee);
            if (resEmp == null)
                return NotFound();

            return printEmployee(resEmp);


        }
    }
}