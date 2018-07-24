using System.Threading.Tasks;
using MediatR;
using System;
using System.Threading;
using LagencyUser.Application.Commands;
using LagencyUserApplication.Model;
using LagencyUser.Application.Contracts;
using System.Linq;

namespace LagencyUser.Application.CommandHandlers
{
    public class ClientHandlers :
    IRequestHandler<CreateClientCommand, Client>,
    IRequestHandler<UpdateClientCommand>
    {
        private readonly IClientRepository _repository;
        public ClientHandlers(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Client> Handle(CreateClientCommand message, CancellationToken cancellationToken)
        {
            var api = new Client
            {
                Id = Guid.NewGuid(),
                Enabled = message.Enabled,
                ClientId = message.ClientId,
                RequireClientSecret = message.RequireClientSecret,
                ClientName = message.ClientName,
                Description = message.Description,
                ClientUri = message.ClientUri,
                LogoUri = message.LogoUri,
                RequireConsent = message.RequireConsent,
                AllowRememberConsent = message.AllowRememberConsent,
                AlwaysIncludeUserClaimsInIdToken = message.AlwaysIncludeUserClaimsInIdToken,
                AllowAccessTokensViaBrowser = message.AllowAccessTokensViaBrowser,
                RedirectUris = message.RedirectUris.Select(c => new ClientRedirectUri() { RedirectUri = c}).ToList(),
                AllowedCorsOrigins = message.RedirectUris.Select(c => new ClientCorsOrigin() { Origin = c }).ToList(),
                IdentityTokenLifetime = message.IdentityTokenLifetime,
                AccessTokenLifetime = message.AccessTokenLifetime,
                AccessTokenType = message.AccessTokenType,
                IncludeJwtId = message.IncludeJwtId
            };

            await _repository.Add(api);
            return api;
        }

        public async Task Handle(UpdateClientCommand message, CancellationToken cancellationToken)
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
            client.RedirectUris = message.RedirectUris.Select(c => new ClientRedirectUri() { RedirectUri = c }).ToList();
            client.AllowedCorsOrigins = message.RedirectUris.Select(c => new ClientCorsOrigin() { Origin = c }).ToList();
            client.IdentityTokenLifetime = message.IdentityTokenLifetime;
            client.AccessTokenLifetime = message.AccessTokenLifetime;
            client.AccessTokenType = message.AccessTokenType;
            client.IncludeJwtId = message.IncludeJwtId;

            await _repository.SaveAsync(client);
        }
    }
}    