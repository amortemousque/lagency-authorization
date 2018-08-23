using System.Threading.Tasks;
using MediatR;
using System;
using System.Threading;
using LagencyUser.Application.Commands;
using LagencyUser.Application.Contracts;
using System.Linq;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using IModels = IdentityServer4.Models;
using LagencyUser.Application.Service;
using IdentityServer4;

namespace LagencyUser.Application.CommandHandlers
{
    public class TenantHandlers :
    IRequestHandler<CreateTenantCommand, Tenant>,
    IRequestHandler<UpdateTenantCommand, bool>,
    IRequestHandler<DeleteTenantCommand, bool>

    {
        private readonly ITenantRepository _repository;

        public TenantHandlers(ITenantRepository repository)
        {
            _repository = repository;
        }

        public async Task<Tenant> Handle(CreateTenantCommand message, CancellationToken cancellationToken)
        {
            var tenant = await Tenant.Factory.CreateNewEntry(_repository, message.Name, message.Description);
            await _repository.Add(tenant);
            return tenant;
        }

        public async Task<bool> Handle(UpdateTenantCommand message, CancellationToken cancellationToken)
        {
            var tenant = await _repository.GetById(message.Id) ?? throw new KeyNotFoundException();

            tenant.UpdateInfos(message.Description, message.LogoUri);
            if (message.Enabled) {
                tenant.Enable();
            } else {
                tenant.Disable();
            }

            await _repository.SaveAsync(tenant);
            return true;
        }

        public async Task<bool> Handle(DeleteTenantCommand message, CancellationToken cancellationToken)
        {
            var tenant = await _repository.GetById(message.Id) ?? throw new KeyNotFoundException();
            await _repository.Delete(message.Id);
            return true;

        }
    }
}        