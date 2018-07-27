using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LagencyUser.Application.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LagencyUser.Application.Contracts
{
    public interface IApiResourceRepository 
    {
        Task<ApiResource> GetById(Guid id);

        Task<List<ApiResource>> List();

        Task<IQueryable<ApiResource>> GetAll();

        Task Add(ApiResource entity);

        Task SaveAsync(ApiResource entity);
    }
}
        