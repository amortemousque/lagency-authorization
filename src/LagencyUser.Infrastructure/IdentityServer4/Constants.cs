// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace LagencyUser.Infrastructure.IdentityServer4
{
    public class Constants
    {
        public class TableNames
        {
            // Configuration
            public const string IdentityResource = "IdentityResources";
            public const string IdentityClaim = "IdentityClaims";

            public const string ApiResource = "ApiResources";
            public const string ApiSecret = "ApiSecrets";
            public const string ApiScope = "ApiScopes";
            public const string ApiClaim = "ApiClaims";
            public const string ApiScopeClaim = "ApiScopeClaims";
            
            public const string Client = "Clients";
            public const string ClientGrantType = "ClientGrantTypes";
            public const string ClientRedirectUri = "ClientRedirectUris";
            public const string ClientPostLogoutRedirectUri = "ClientPostLogoutRedirectUris";
            public const string ClientScopes = "ClientScopes";
            public const string ClientSecret = "ClientSecrets";
            public const string ClientClaim = "ClientClaims";
            public const string ClientIdPRestriction = "ClientIdPRestrictions";
            public const string ClientCorsOrigin = "ClientCorsOrigins";

            // Operational
            public const string PersistedGrant = "PersistedGrants";
        }

        public static class CustomClaimTypes
        {
            public const string TenantId = "tenant_id";

            public const string TenantName = "tenant_name";

            public const string Permission = "permission";

        }



        public static class CustomScopes
        {
            public const string Tenant = "tenant";

            public const string Scope = "scope";

            public const string Role = "role";
        }

    }
}