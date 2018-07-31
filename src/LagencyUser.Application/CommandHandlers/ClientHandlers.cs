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
            var client = new Client
            {
                Id = Guid.NewGuid(),
                ClientName = message.ClientName,
                ClientTypeId = message.ClientTypeId,
                ClientId = ClientIdGenerator.Generate(32),
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId  
                }
            };



            if(message.ClientTypeId == ClientType.SinglePage.Id) 
            {
                client.RequireConsent = false;
                client.AllowedGrantTypes = IModels.GrantTypes.Implicit.ToList();
                client.AccessTokenType = (int) IModels.AccessTokenType.Jwt;
                client.AllowAccessTokensViaBrowser = true;
                client.AlwaysIncludeUserClaimsInIdToken = true;
            } 
            else if (message.ClientTypeId == ClientType.MachineToMachine.Id) 
            {
                client.RequireConsent = false;
                client.AllowedGrantTypes = IModels.GrantTypes.ClientCredentials.ToList();
                client.ClientSecrets = new List<ClientSecret>  {
                    new ClientSecret(IModels.HashExtensions.Sha256("secret"))
                };
                client.RequireClientSecret = true;
            }

            await _repository.Add(client);
            return client;
        }

        public async Task<bool> Handle(UpdateClientCommand message, CancellationToken cancellationToken)
        {
            var client = await _repository.GetById(message.Id) ?? throw new KeyNotFoundException();

            message.AllowedScopes = message.AllowedScopes ?? new List<string>();

            //message.AllowedScopes.Add(IdentityServerConstants.StandardScopes.OpenId);

            client.Enabled = message.Enabled;
            client.RequireClientSecret = message.RequireClientSecret;
            client.ClientName = message.ClientName;
            client.Description = message.Description;
            client.ClientUri = message.ClientUri;
            client.LogoUri = message.LogoUri;
            client.RequireConsent = message.RequireConsent;
            client.AlwaysIncludeUserClaimsInIdToken = message.AlwaysIncludeUserClaimsInIdToken;
            client.AllowAccessTokensViaBrowser = message.AllowAccessTokensViaBrowser;
            client.RedirectUris = message.RedirectUris;
            client.AllowedScopes = message.AllowedScopes;
            client.AllowedCorsOrigins = message.AllowedCorsOrigins;
            client.IdentityTokenLifetime = message.IdentityTokenLifetime;
          
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