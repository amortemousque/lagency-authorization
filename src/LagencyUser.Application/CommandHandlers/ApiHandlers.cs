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
    IRequestHandler<DeleteApiCommand, bool>,
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
                DisplayName = message.DisplayName,
                Enabled = true
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

        public async Task<bool> Handle(DeleteApiCommand message, CancellationToken cancellationToken)
        {
            var client = await _repository.GetById(message.Id) ?? throw new KeyNotFoundException();
            await _repository.Delete(message.Id);
            return true;

        }


        //Api Scope
        public async Task<ApiScope> Handle(CreateApiScopeCommand message, CancellationToken cancellationToken)
        {
            var api = await _repository.GetById(message.ApiResourceId) ?? throw new KeyNotFoundException();

            var apiScope = new ApiScope
            {
                Id = Guid.NewGuid(),
                ApiResourceId = api.Id,
                Name = message.Name,
                DisplayName = message.Name,
                Description = message.Description
            };

            api.Scopes.Add(apiScope);

            await _repository.SaveAsync(api);
            return apiScope;
        }

        public async Task<bool> Handle(UpdateApiScopeCommand message, CancellationToken cancellationToken)
        {
            var api = await _repository.GetById(message.ApiResourceId) ?? throw new KeyNotFoundException();
            var scope = api.Scopes.FirstOrDefault(s => s.Id == message.Id) ?? throw new KeyNotFoundException();
            scope.Name = message.Name;
            scope.DisplayName = message.Name;
            scope.Description = message.Description;

            await _repository.SaveAsync(api);
            return true;
        }

        public async Task<bool> Handle(DeleteApiScopeCommand message, CancellationToken cancellationToken)
        {
            var api = await _repository.GetById(message.ApiResourceId) ?? throw new KeyNotFoundException();
            var scope = api.Scopes.FirstOrDefault(s => s.Id == message.Id) ?? throw new KeyNotFoundException();
            api.Scopes.Remove(scope);
            await _repository.SaveAsync(api);
            return true;

        }
    }
}