using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagencyUser.Application.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LagencyUser.Application.Contracts
{
    public interface IRoleRepository
    {

        Task<IQueryable<IdentityRole>> GetAll();

        Task<List<string>> GetRolePermissions(string[] roleNames);

        Task<List<Permission>> GetRolePermissions(Guid id);

        Task<bool> HasUniqName(string name);
    }
}
    