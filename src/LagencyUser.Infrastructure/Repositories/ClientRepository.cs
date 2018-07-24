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
    public class ClientRepository : IClientRepository
    {
        private readonly DbContext _context;

        public ClientRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetById(Guid id)
        {
            return await _context.Clients.AsQueryable<Client>().SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Client>> List()
        {
            return await _context.Clients.Find(_ => true).ToListAsync();
        }

        public async Task Add(Client entity)
        {
            await _context.Clients.InsertOneAsync(entity);
        }

        public async Task SaveAsync(Client entity)
        {
            await _context.Clients.ReplaceOneAsync(doc => doc.Id == entity.Id, entity, new UpdateOptions() { IsUpsert = true });
        }
    }
}
        