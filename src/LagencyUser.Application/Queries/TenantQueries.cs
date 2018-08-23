﻿using System;
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
    public class TenantQueries
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantQueries(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }


        public async Task<Tenant> GetTenantByIdAsync(Guid id)
        {
            var response = await _tenantRepository.GetById(id);

            if (response == null)
                throw new KeyNotFoundException();

            return response;
        }

        public async Task<Tenant> GetTenantByNameAsync(string name)
        {
            var response = await _tenantRepository.GetByName(name);

            if (response == null)
                throw new KeyNotFoundException();

            return response;
        }

        public async Task<List<Tenant>> GetTenantsAsync(string name, bool? enabled)
        {

            var response = await _tenantRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(name))
            {
                response = response.Where(a => a.Name.StartsWith(name));
            }

            if (enabled != null)
            {
                response = response.Where(a => a.Enabled == enabled);
            }


            return response.ToList();
        }

    }

}
