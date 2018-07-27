using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class CreateClientCommand : IRequest<Client>
    {
        public int ClientTypeId { get; set; }

        public bool Enabled { get; set; } = true;

        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string Description { get; set; }

        public string ClientUri { get; set; }

        public string LogoUri { get; set; }

        public bool RequireConsent { get; set; }

        public bool AllowRememberConsent { get; set; }

        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        public bool AllowAccessTokensViaBrowser { get; set; }

        public List<string> RedirectUris { get; set; }

        public int TokenLifetime { get; set; }


        public List<string> AllowedCorsOrigins { get; set; }
    }
}
        