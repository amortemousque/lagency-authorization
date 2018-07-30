using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagencyUser.Application.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LagencyUser.Application.Contracts
{
    public interface IClientRepository 
    {
        Task<Client> GetById(Guid id);

        Task<List<Client>> List();

        Task<IQueryable<Client>> GetAll();

        Task Add(Client entity);

        Task Delete(Guid id);

        Task SaveAsync(Client entity);
    }
}
            