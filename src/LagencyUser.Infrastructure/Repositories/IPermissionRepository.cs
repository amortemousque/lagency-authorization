﻿using System;
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
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Permission> GetById(Guid id)
        {
            return await _context.Permissions.AsQueryable().SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Permission>> GetByNames(string[] names) 
        {
            var filterDef = new FilterDefinitionBuilder<Permission>();
            var filter = filterDef.In(x => x.Name, names);
            var permissions = await _context.Permissions.Find(filter).ToListAsync();
            return permissions;
        }


        public async Task<List<Permission>> List()
        {
            return await _context.Permissions.Find(_ => true).ToListAsync();
        }


        public async Task<IQueryable<Permission>> GetAll()
        {
            return _context.Permissions.AsQueryable();
        }

        public async Task Add(Permission entity)
        {
            await _context.Permissions.InsertOneAsync(entity);
        }

        public async Task Delete(Guid id)
        {
            await _context.Permissions.DeleteOneAsync(c => c.Id == id);
        }

        public async Task SaveAsync(Permission entity)
        {
            await _context.Permissions.ReplaceOneAsync(doc => doc.Id == entity.Id, entity, new UpdateOptions() { IsUpsert = true });
        }

        public async Task<bool> HasUniqName(string name)
        {
            return !await _context.Permissions.AsQueryable().AnyAsync(a => a.Name.ToLower() == name.ToLower());
        }
    }
}
