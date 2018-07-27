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

namespace LagencyUser.Application.CommandHandlers
{
    public class ClientHandlers :
    IRequestHandler<CreateClientCommand, Client>,
    IRequestHandler<UpdateClientCommand, bool>
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
                Description = message.Description,
                Enabled = message.Enabled,
                ClientName = message.ClientName,
                ClientUri = message.ClientUri,
                LogoUri = message.LogoUri,
                AllowedCorsOrigins = message.RedirectUris,
                IdentityTokenLifetime = message.TokenLifetime,
                AccessTokenLifetime = message.TokenLifetime
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
                client.RedirectUris = message.RedirectUris;
            }

            await _repository.Add(client);
            return client;
        }

        public async Task<bool> Handle(UpdateClientCommand message, CancellationToken cancellationToken)
        {
            var client = await _repository.GetById(message.Id);

            client.Enabled = message.Enabled;
            client.ClientId = message.ClientId;
            client.RequireClientSecret = message.RequireClientSecret;
            client.ClientName = message.ClientName;
            client.Description = message.Description;
            client.ClientUri = message.ClientUri;
            client.LogoUri = message.LogoUri;
            client.RequireConsent = message.RequireConsent;
            client.AllowRememberConsent = message.AllowRememberConsent;
            client.AlwaysIncludeUserClaimsInIdToken = message.AlwaysIncludeUserClaimsInIdToken;
            client.AllowAccessTokensViaBrowser = message.AllowAccessTokensViaBrowser;
            client.RedirectUris = message.RedirectUris;
            client.AllowedCorsOrigins = message.RedirectUris;
            client.IdentityTokenLifetime = message.IdentityTokenLifetime;
            client.AccessTokenLifetime = message.AccessTokenLifetime;
            client.AccessTokenType = message.AccessTokenType;
            client.IncludeJwtId = message.IncludeJwtId;

            await _repository.SaveAsync(client);
            return true;

        }

        //Task<Unit> IRequestHandler<UpdateClientCommand, Unit>.Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}
    }
}    