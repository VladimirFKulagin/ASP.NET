﻿using System;
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

        public Task<T> Create(T employee)
        {
            Data.Add(employee);
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == employee.Id));
        }

        public Task<T> Remove(Guid id)
        {
            var removedEntity = Data.FirstOrDefault(x => x.Id == id);
            if (removedEntity!=null)
                Data.Remove(removedEntity);
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task<T> Update(Guid id, T employee)
        {
            Data = Data.Select(x => (x.Id == id)?employee:x).ToList();
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }
    }
}