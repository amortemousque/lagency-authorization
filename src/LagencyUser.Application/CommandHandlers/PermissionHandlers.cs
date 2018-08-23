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
using LagencyUser.Application.Events;

namespace LagencyUser.Application.CommandHandlers
{
    public class PermissionHandlers :
    IRequestHandler<CreatePermissionCommand, Permission>,
    IRequestHandler<UpdatePermissionCommand, bool>,
    IRequestHandler<DeletePermissionCommand, bool>

    {
        private readonly IPermissionRepository _repository;
        private readonly IMediator _mediator;

        public PermissionHandlers(IPermissionRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<Permission> Handle(CreatePermissionCommand message, CancellationToken cancellationToken)
        {
            var permission = await Permission.Factory.CreateNewEntry(_repository, message.Name, message.Description);
            await _repository.Add(permission);
            return permission;
        }

        public async Task<bool> Handle(UpdatePermissionCommand message, CancellationToken cancellationToken)
        {
            var permission = await _repository.GetById(message.Id) ?? throw new KeyNotFoundException();

            permission.UpdateInfos(message.Description);


            await _repository.SaveAsync(permission);
            return true;
        }

        public async Task<bool> Handle(DeletePermissionCommand message, CancellationToken cancellationToken)
        {
            var permission = await _repository.GetById(message.Id) ?? throw new KeyNotFoundException();
            await _repository.Delete(message.Id);
            await _mediator.Publish(new PermissionDeletedEvent(permission.Id, permission.Name));

            return true;

        }

    }
}