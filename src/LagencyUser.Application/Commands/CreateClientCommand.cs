using System;
using System.Collections.Generic;
using LagencyUserApplication.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class CreateClientCommand : IRequest<Client>
    {
        public bool Enabled { get; set; } = true;

        public string ClientId { get; set; }

        public bool RequireClientSecret { get; set; } = true;

        public string ClientName { get; set; }

        public string Description { get; set; }

        public string ClientUri { get; set; }

        public string LogoUri { get; set; }

        public bool RequireConsent { get; set; }

        public bool AllowRememberConsent { get; set; }

        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        public bool AllowAccessTokensViaBrowser { get; set; }

        public List<string> RedirectUris { get; set; }

        public int IdentityTokenLifetime { get; set; }

        public int AccessTokenLifetime { get; set; }

        public int AccessTokenType { get; set; } // AccessTokenType.Jwt;

        public bool IncludeJwtId { get; set; }

        public List<string> AllowedCorsOrigins { get; set; }
    }
}
        