using System;
using System.Security.Claims;
using System.Threading.Tasks;
using LagencyUserApplication.Model;
using Microsoft.AspNetCore.Identity;

namespace LagencyUserInfrastructure.Identity
{
    public class UserClaimsPrincipalFactory : IUserClaimsPrincipalFactory<IdentityUser>
    {

        public async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await CreateAsync(user);
            return principal;
        }
    }
}
