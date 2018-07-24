using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagencyUserApplication.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LagencyUser.Application.Contracts
{
    public interface IApiResourceRepository 
    {
        Task<ApiResource> GetById(Guid id);

        Task<List<ApiResource>> List();

        Task Add(ApiResource entity);

        Task SaveAsync(ApiResource entity);
    }
}
        