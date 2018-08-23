using System;
using IdentityModel;
using IdentityServer4.Models;
using LagencyUser.Infrastructure.IdentityServer4;
using static LagencyUser.Infrastructure.IdentityServer4.Constants;

namespace LagencyUser.Infrastructure.IdentityServer4.Models
{
    public static class CustomIdentityResources
    {
        
        public class Tenant : IdentityResource
        {
            public Tenant(): base("tenant", new[] { CustomClaimTypes.TenantId, CustomClaimTypes.TenantName }) 
            {
                this.DisplayName = "Tenant informations";
            }
        }

        public class Role : IdentityResource
        {
            public Role() : base("role", new[] { JwtClaimTypes.Role })
            {
                this.DisplayName = "User role list";
            }
        }

        public class Scope : IdentityResource
        {
            public Scope() : base("scope", new[] { JwtClaimTypes.Scope })
            {
                this.DisplayName = "User api scope list";
            }
        }

    }
}
