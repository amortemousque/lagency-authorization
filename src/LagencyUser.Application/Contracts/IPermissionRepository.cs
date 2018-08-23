using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagencyUser.Application.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LagencyUser.Application.Contracts
{
    public interface IPermissionRepository 
    {
        Task<Permission> GetById(Guid id);

        Task<List<Permission>> GetByNames(string[] ids);

        Task<List<Permission>> List();

        Task<IQueryable<Permission>> GetAll();

        Task Add(Permission entity);

        Task Delete(Guid id);

        Task SaveAsync(Permission entity);

        Task<bool> HasUniqName(string name);
    }
}
                    