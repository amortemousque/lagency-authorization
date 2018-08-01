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
    IRequestHandler<DeleteApiCommand, bool>,
    IRequestHandler<UpdateApiCommand, bool>,
    IRequestHandler<CreateApiScopeCommand, ApiScope>,
    IRequestHandler<UpdateApiScopeCommand, bool>,
    IRequestHandler<DeleteApiScopeCommand, bool>
    {
        private readonly IApiResourceRepository _repository;
        public ApiHandlers(IApiResourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResource> Handle(CreateApiCommand message, CancellationToken cancellationToken)
        {
            var api = await ApiResource.Factory.CreateNewEntry(_repository, message.Name, message.DisplayName);
            await _repository.Add(api);
            return api;
        }



        public async Task<bool> Handle(UpdateApiCommand message, CancellationToken cancellationToken)
        {
            var api = await _repository.GetById(message.Id) ?? throw new KeyNotFoundException();
            api.UpdateInfos(message.DisplayName, message.Description, message.UserClaims);

            if (message.Enabled)
            {
                api.Enable();
            }
            else
            {
                api.Disable();
            }
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
            var scope = api.AddScope(message.Name, message.Description);
            await _repository.SaveAsync(api);
            return scope;
        }

        public async Task<bool> Handle(UpdateApiScopeCommand message, CancellationToken cancellationToken)
        {
            var api = await _repository.GetById(message.ApiResourceId) ?? throw new KeyNotFoundException();
            var scope = api.Scopes.FirstOrDefault(s => s.Id == message.Id) ?? throw new KeyNotFoundException();
            scope.Description = message.Description;

            await _repository.SaveAsync(api);
            return true;
        }

        public async Task<bool> Handle(DeleteApiScopeCommand message, CancellationToken cancellationToken)
        {
            var api = await _repository.GetById(message.ApiResourceId) ?? throw new KeyNotFoundException();
            api.DeleteScope(message.Id);

            await _repository.SaveAsync(api);
            return true;

        }
    }
}