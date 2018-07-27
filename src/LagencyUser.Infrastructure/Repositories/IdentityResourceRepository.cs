using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagencyUser.Application.Contracts;
using LagencyUser.Application.Model;
using LagencyUser.Infrastructure.Context;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LagencyUser.Infrastructure.Repositories
{
    public class IdentityResourceRepository : IIdentityResourceRepository
    {
        private readonly ApplicationDbContext _context;

        public IdentityResourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IdentityResource> GetById(Guid id)
        {
            return await _context.IdentityResources.AsQueryable<IdentityResource>().SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<IdentityResource>> List()
        {
            return await _context.IdentityResources.Find(_ => true).ToListAsync();
        }

        public async Task Add(IdentityResource entity)
        {
            await _context.IdentityResources.InsertOneAsync(entity);
        }

        public async Task SaveAsync(IdentityResource entity)
        {
            await _context.IdentityResources.ReplaceOneAsync(doc => doc.Id == entity.Id, entity, new UpdateOptions() { IsUpsert = true });
        }
    }
}
            