using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public void Create(T employee)
        {
            List<T> newEmployeeList = Data.ToList();
            newEmployeeList.Add(employee);
            Data = newEmployeeList;
        }

        public void Remove(Guid id)
        {
            Data = Data.Where(x => x.Id != id);
        }

        public void Update(Guid id, T employee)
        {
            var person = Data.FirstOrDefault(x => x.Id == id) as Employee;
            if (person != null)
            {
                var emp = employee as Employee;
                person.Email = emp.Email;
                person.FirstName = emp.FirstName;
                person.LastName = emp.LastName;
                person.Roles = emp.Roles;
                person.AppliedPromocodesCount = emp.AppliedPromocodesCount;
            }
        }
    }
}