using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagencyUser.Application.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LagencyUser.Application.Contracts
{
    public interface ITenantRepository 
    {
        Task<Tenant> GetById(Guid id);

        Task<Tenant> GetByName(string name);

        Task<List<Tenant>> List();

        Task<IQueryable<Tenant>> GetAll();

        Task Add(Tenant entity);

        Task Delete(Guid id);

        Task SaveAsync(Tenant entity);

        Task<bool> HasUniqName(string name);
    }
}
                