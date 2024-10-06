using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.WebHost.Models.DTO;

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
            return PrintEmployee(employee);
        }

        /// <summary>
        /// Создать нового сотрудника
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployee(EmployeeDtoCreate employee)
        {
            var resEmp = await _employeeRepository.Create(employee.ToEmployee());
            if (resEmp == null)
                return NotFound("Failed to create employee");
            return PrintEmployee(resEmp);
        }

        /// <summary>
        /// Удалить сотрудника по id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task<ActionResult<EmployeeResponse>> RemoveEmployee(Guid id)
        {
            var resEmp = await _employeeRepository.Remove(id);
            if (resEmp == null)
                return Ok("Employee was removed");
            else
                return NotFound($"Employee with ID {id} not found");
        }

        /// <summary>
        /// Обновить данные о сотруднике с указанным id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        [HttpPatch]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployee(Guid id, EmployeeDtoCreate employee)
        {
            var existingEmp = await _employeeRepository.GetByIdAsync(id);
            if (existingEmp == null)
                return NotFound($"Employee with ID {id} not found");
            var resEmp = await _employeeRepository.Update(id, employee.ToEmployee(id));
            if (resEmp == null)
                return NotFound();
            return PrintEmployee(resEmp);


        }

        private EmployeeResponse PrintEmployee(Employee emp)
        {
            var employeeModel = new EmployeeResponse()
            {
                Id = emp.Id,
                Email = emp.Email,
                Roles = emp.Roles.Select(x => new RoleItemResponse()
                {
                    Id =x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = emp.FullName,
                AppliedPromocodesCount = emp.AppliedPromocodesCount
            };
            return employeeModel;
        }
    }
}