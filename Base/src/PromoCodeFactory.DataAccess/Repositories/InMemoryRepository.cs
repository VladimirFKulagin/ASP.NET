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
        protected IList<T> Data { get; set; }

        public InMemoryRepository(IList<T> data)
        {
            Data = data;
        }

        public Task<IList<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task Create(T employee)
        {
            Data.Add(employee);
            return Task.CompletedTask;
        }

        public Task Remove(Guid id)
        {
            var removedEntity = Data.FirstOrDefault(x => x.Id == id);
            Data.Remove(removedEntity);
            return Task.CompletedTask;
        }

        public Task Update(Guid id, T employee)
        {
            Employee person = Data.FirstOrDefault(x => x.Id == id) as Employee;
            if (person != null)
            {
                var emp = employee as Employee;
                person.Email = emp.Email;
                person.FirstName = emp.FirstName;
                person.LastName = emp.LastName;
                person.Roles = emp.Roles;
                person.AppliedPromocodesCount = emp.AppliedPromocodesCount;
            }
            return Task.CompletedTask;
        }
    }
}