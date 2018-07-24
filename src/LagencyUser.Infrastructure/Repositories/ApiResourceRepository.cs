using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagencyUser.Application.Contracts;
using LagencyUserApplication.Model;
using LagencyUserInfrastructure.Context;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LagencyUser.Infrastructure.Repositories
{
    public class ApiResourceRepository : IApiResourceRepository
    {
        private readonly DbContext _context;

        public ApiResourceRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<ApiResource> GetById(Guid id)
        {
            return await _context.ApiResources.AsQueryable<ApiResource>().SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<ApiResource>> List()
        {
            return await _context.ApiResources.Find(_ => true).ToListAsync();
        }

        public async Task Add(ApiResource entity)
        {
            await _context.ApiResources.InsertOneAsync(entity);
        }

        public async Task SaveAsync(ApiResource entity)
        {
            await _context.ApiResources.ReplaceOneAsync(doc => doc.Id == entity.Id, entity, new UpdateOptions() { IsUpsert = true });
        }
    }
}
    