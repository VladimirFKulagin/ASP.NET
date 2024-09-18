using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        /// <summary>
        /// Добавить сотрудника
        /// </summary>
        /// <param name="employee"></param>
        [HttpPost]
        public async Task CreateEmployee([FromBody] Employee employee)
        {
            if (await _employeeRepository.GetByIdAsync(employee.Id) != null)
                BadRequest("Employee with this ID exist");
            else
                await _employeeRepository.Create(employee);
            
        }

        /// <summary>
        /// Удалить сотрудника по id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task RemoveEmployee(Guid id)
        {
            if (await _employeeRepository.GetByIdAsync(id) != null)
                await _employeeRepository.Remove(id);
            else NotFound("Employee not found");
        }

        /// <summary>
        /// Обновить данные о сотруднике с указанным id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        [HttpPatch]
        public async Task UpdateEmployee(Guid id, [FromBody] Employee employee)
        {
            if (await _employeeRepository.GetByIdAsync(id) != null)
                await _employeeRepository.Update(id, employee);
            else NotFound("Employee not found");
        }
    }
}