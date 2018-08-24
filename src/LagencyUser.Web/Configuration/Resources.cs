// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using LagencyUser.Infrastructure.IdentityServer4.Models;
using System.Collections.Generic;

namespace LagencyUser.Web.Configuration
{
    public class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                // some standard scopes from the OIDC spec
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new CustomIdentityResources.Role(),
                new CustomIdentityResources.Scope(),
                new CustomIdentityResources.Tenant()
            };
               
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                // expanded version if more control is needed
                new ApiResource
                {
                    Name = "apitest",

                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Email
                    },

                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "api.full_access",
                            DisplayName = "Full access to API"
                        },
                        new Scope
                        {
                            Name = "api.read_only",
                            DisplayName = "Read only access to API"
                        }
                    }
                }
            };
        }
    }
}