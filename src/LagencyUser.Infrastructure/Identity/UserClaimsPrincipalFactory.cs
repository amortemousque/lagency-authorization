using System;
using System.Security.Claims;
using System.Threading.Tasks;
using LagencyUser.Application.Model;
using Microsoft.AspNetCore.Identity;

namespace LagencyUser.Infrastructure.Identity
{
    public class UserClaimsPrincipalFactory : IUserClaimsPrincipalFactory<IdentityUser>
    {

        public async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await CreateAsync(user);
            var test = principal.Claims;
            return principal;
        }


    }
}
