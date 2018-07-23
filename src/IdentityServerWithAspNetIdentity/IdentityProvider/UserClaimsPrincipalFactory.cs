using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerWithAspNetIdentity.Identity
{
    public class UserClaimsPrincipalFactory : IUserClaimsPrincipalFactory<IdentityUser>
    {

        public async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await CreateAsync(user);
            //((ClaimsIdentity)principal.Identity).AddClaims(new[] {
            //new Claim(ClaimTypes.GivenName, user.FirstName),
            //new Claim(ClaimTypes.Surname, user.LastName),

            //});
            return principal;
        }
    }
}
