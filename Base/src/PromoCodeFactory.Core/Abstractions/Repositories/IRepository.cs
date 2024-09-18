using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T> where T: BaseEntity
    {
        Task<IList<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task Create(T employee);
        Task Remove(Guid id);
        Task Update(Guid id, T employee);

    }
}