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
    public class ClientHandlers :
    IRequestHandler<CreateClientCommand, Client>,
    IRequestHandler<UpdateClientCommand, bool>,
    IRequestHandler<DeleteClientCommand, bool>

    {
        private readonly IClientRepository _repository;

        public ClientHandlers(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Client> Handle(CreateClientCommand message, CancellationToken cancellationToken)
        {
            var client = await Client.Factory.CreateNewEntry(_repository, message.ClientName, message.ClientTypeId);
            await _repository.Add(client);
            return client;
        }

        public async Task<bool> Handle(UpdateClientCommand message, CancellationToken cancellationToken)
        {
            var client = await _repository.GetById(message.Id) ?? throw new KeyNotFoundException();

            message.AllowedScopes = message.AllowedScopes ?? new List<string>();

            await client.UpdateSettings(
                 _repository,
                 message.ClientName,
                 message.Description,
                 message.ClientUri,
                 message.LogoUri,
                 message.RequireClientSecret,
                 message.RequireConsent,
                 message.AlwaysIncludeUserClaimsInIdToken,
                 message.AllowAccessTokensViaBrowser,
                 message.IdentityTokenLifetime,
                 message.RedirectUris,
                 message.AllowedCorsOrigins,
                 message.AllowedScopes
             );

            if (message.Enabled)
            {
                client.Enable();
            }
            else
            {
                client.Disable();
            }
          
            await _repository.SaveAsync(client);

            return true;
        }

        public async Task<bool> Handle(DeleteClientCommand message, CancellationToken cancellationToken)
        {
            var client = await _repository.GetById(message.Id) ?? throw new KeyNotFoundException();
            await _repository.Delete(message.Id);
            return true;

        }
    }
}    