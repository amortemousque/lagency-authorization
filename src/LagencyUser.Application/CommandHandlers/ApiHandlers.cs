using System.Threading.Tasks;
using MediatR;
using System;
using System.Threading;
using System.Collections.Generic;
using LagencyUser.Application.Commands;
using LagencyUser.Application.Model;
using LagencyUser.Application.Contracts;
using System.Linq;

namespace LagencyUser.Application.CommandHandlers
{
    public class ApiHandlers : 
    IRequestHandler<CreateApiCommand, ApiResource>,
    IRequestHandler<CreateApiScopeCommand, ApiScope>,
    IRequestHandler<UpdateApiCommand, bool>,
    IRequestHandler<UpdateApiScopeCommand, bool>

    {
        private readonly IApiResourceRepository _repository;
        public ApiHandlers(IApiResourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResource> Handle(CreateApiCommand message, CancellationToken cancellationToken)
        {
            var api = new ApiResource
            {
                Id = Guid.NewGuid(),
                Name = message.Name,
                Description = message.Description,
                DisplayName = message.DisplayName,
                Enabled = message.Enabled,
                UserClaims = message.UserClaims
            };

            await _repository.Add(api);
            return api;
        }



        public async Task<bool> Handle(UpdateApiCommand message, CancellationToken cancellationToken)
        {
            var api = await _repository.GetById(message.Id);

            api.Name = message.Name;
            api.Enabled = message.Enabled;
            api.DisplayName = message.DisplayName;
            api.Description = message.Description;
            api.UserClaims = message.UserClaims;

            await _repository.SaveAsync(api);
            return true;
        }


        public async Task<ApiScope> Handle(CreateApiScopeCommand message, CancellationToken cancellationToken)
        {
            var api = await _repository.GetById(message.ApiResourceId);

            var apiScope = new ApiScope
            {
                Name = message.Name,
                DisplayName = message.DisplayName,
                Description = message.Description
            };

            api.Scopes.Add(apiScope);

            await _repository.Add(api);
            return apiScope;
        }

        public async Task<bool> Handle(UpdateApiScopeCommand message, CancellationToken cancellationToken)
        {
            var api = await _repository.GetById(message.ApiResourceId);
            var scope = api.Scopes.FirstOrDefault(s => s.Name.ToLower() == message.Name.ToLower());
            scope.DisplayName = message.DisplayName;
            scope.Description = message.Description;

            await _repository.SaveAsync(api);
            return true;
        }

    }
}