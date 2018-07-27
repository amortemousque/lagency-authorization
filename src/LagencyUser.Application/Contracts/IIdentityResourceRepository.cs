using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagencyUser.Application.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LagencyUser.Application.Contracts
{
    public interface IIdentityResourceRepository 
    {
        Task<IdentityResource> GetById(Guid id);

        Task<List<IdentityResource>> List();

        Task Add(IdentityResource entity);

        Task SaveAsync(IdentityResource entity);
    }
}
                    