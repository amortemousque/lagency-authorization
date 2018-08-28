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
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<IdentityRole>> GetAll()
        {
            return _context.Roles.AsQueryable();
        }


        public async Task<List<string>> GetRolePermissions(string[] roleNames)
        {
            var filterDef = new FilterDefinitionBuilder<IdentityRole>();
            var filter = filterDef.In(x => x.NormalizedName, roleNames);
            var roles = await _context.Roles.Find(filter).ToListAsync() ?? new List<IdentityRole>();
            var permissions = roles.ToList().SelectMany(r => r.Permissions ?? new List<string>()).Distinct().ToList();

            return permissions;
        }

        public async Task<List<Permission>> GetRolePermissions(Guid id)
        {
            var role = await _context.Roles.AsQueryable().SingleOrDefaultAsync(t => t.Id == id.ToString());

            var rolePermissions = role.Permissions ?? new List<string>();


            var filterDef = new FilterDefinitionBuilder<Permission>();
            var filter = filterDef.In(x => x.Name, rolePermissions);
            var permissions = await _context.Permissions.Find(filter).ToListAsync();

            return permissions;
        }

        public async Task<bool> HasUniqName(string name)
        {
            return !await _context.Roles.AsQueryable().AnyAsync(a => a.Name.ToLower() == name.ToLower());
        }
    }
}
    