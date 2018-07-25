using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LagencyUser.Application.Contracts;
using LagencyUserApplication.Model;
using System.Linq.Expressions;

namespace LagencyUser.Application.Queries
{
    public class ApiQueries
    {
        private readonly IApiResourceRepository _apiResourceRepository;

        public ApiQueries(IApiResourceRepository apiResourceRepository)
        {
            _apiResourceRepository = apiResourceRepository;
        }


        public async Task<ApiResource> GetApiAsync(Guid apiId)
        {
            var response = await _apiResourceRepository.GetById(apiId);

            if (response == null)
                throw new KeyNotFoundException();

            return response;
        }

        public async Task<List<ApiResource>> GetApisAsync(string name, string displayName, bool? enabled)
        {
            
            var response = await _apiResourceRepository.GetAll();
                                                          
            if(!string.IsNullOrWhiteSpace(name)) {
                response = response.Where(a => a.Name.StartsWith(name));
            }

            if(!string.IsNullOrWhiteSpace(displayName)) {
                response = response.Where(a => a.DisplayName.StartsWith(displayName));
            }

            if (enabled != null)
            {
                response = response.Where(a => a.Enabled == enabled);
            }


            return response.ToList();
        }
            
    }

}
    