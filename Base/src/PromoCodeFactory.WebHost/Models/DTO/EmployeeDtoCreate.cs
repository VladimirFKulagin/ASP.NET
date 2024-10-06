using PromoCodeFactory.Core.Domain.Administration;
using System;

using System.Collections.Generic;

namespace PromoCodeFactory.WebHost.Models.DTO
{
    public class EmployeeDtoCreate
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required List<Role> Roles { get; set; }
        public int AppliedPromocodesCount { get; set; }

        public Employee ToEmployee()
        {
            var newEmp = new Employee()
            {
                Id = Guid.NewGuid(),
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Roles = Roles,
                AppliedPromocodesCount = AppliedPromocodesCount
            };
            return newEmp;
        }
        public Employee ToEmployee(Guid id)
        {
            var newEmp = new Employee()
            {
                Id = id,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Roles = Roles,
                AppliedPromocodesCount = AppliedPromocodesCount
            };
            return newEmp;
        }
    }
}
