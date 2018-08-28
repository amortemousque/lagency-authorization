using System;
using IdentityServer4;

namespace LagencyUser.Application.Model
{
    public class AllowedScope : Enumeration
    {
        public static AllowedScope OpenId = new AllowedScope(1, IdentityServerConstants.StandardScopes.OpenId);
        public static AllowedScope Profile = new AllowedScope(2, IdentityServerConstants.StandardScopes.Profile);
        public static AllowedScope Email = new AllowedScope(3, IdentityServerConstants.StandardScopes.Email);
        public static AllowedScope Address = new AllowedScope(4, IdentityServerConstants.StandardScopes.Address);
        public static AllowedScope Phone = new AllowedScope(5, IdentityServerConstants.StandardScopes.Phone);
        public static AllowedScope Role = new AllowedScope(6, "role");
        public static AllowedScope Scope = new AllowedScope(7, "scope");
        public static AllowedScope Tenant = new AllowedScope(8, "tenant");
        public static AllowedScope Permission = new AllowedScope(9, "permission");


        protected AllowedScope() { }

        public AllowedScope(int id, string name)
            : base(id, name)
        {
        }
    }
}
