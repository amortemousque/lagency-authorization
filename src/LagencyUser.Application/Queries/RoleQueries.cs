using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LagencyUser.Application.Contracts;
using LagencyUser.Application.Model;
using System.Linq.Expressions;

namespace LagencyUser.Application.Queries
{
    public class RoleQueries
    {
        private readonly IRoleRepository _roleRepository;

        public RoleQueries(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        //public async Task<Client> GetRoleAsync(Guid id)
        //{
        //    var response = await _roleRepository.GetById(id);

        //    if (response == null)
        //        throw new KeyNotFoundException();

        //    return response;
        //}

        public async Task<List<IdentityRole>> GetRolesAsync(string name)
        {

            var response = await _roleRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(name))
            {
                var nameUpper = name.ToUpper();
                response = response.Where(a => a.Name.ToLower().Equals(name));
            }

            return response.ToList();
        }

        public async Task<List<Permission>> GetRolePermissionsAsync(Guid id)
        {
            var response = await _roleRepository.GetRolePermissions(id);
            return response.ToList();
        }
    }
}
