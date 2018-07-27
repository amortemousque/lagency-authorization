using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagencyUser.Application.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LagencyUser.Application.Contracts
{
    public interface IPersistedGrandRepository 
    {
        Task<PersistedGrant> GetById(Guid id);

        Task<List<PersistedGrant>> List();

        Task Add(PersistedGrant entity);

        Task SaveAsync(PersistedGrant entity);
    }
}
                