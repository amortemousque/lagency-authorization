
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model = LagencyUser.Application.Model;

namespace LagencyUser.Web
{
    public class CustomSignInManager : SignInManager<Model.IdentityUser>
    {
        public CustomSignInManager(UserManager<Model.IdentityUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<Model.IdentityUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<Model.IdentityUser>> logger, IAuthenticationSchemeProvider schemes)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            var result = await base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
            if(result.Succeeded) {
                var user = await UserManager.FindByNameAsync(userName);
                user.LastLoginDate = DateTime.Now;
                await UserManager.UpdateAsync(user);
            }
            return result;
        }

        public override async Task SignInAsync(Model.IdentityUser user, bool isPersistent, string authenticationMethod = null)
        {
            await base.SignInAsync(user, isPersistent, authenticationMethod);
            user.LastLoginDate = DateTime.Now;
            await UserManager.UpdateAsync(user);
        }

        public override async Task SignInAsync(Model.IdentityUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null) 
        {
            await base.SignInAsync(user, authenticationProperties, authenticationMethod);
            user.LastLoginDate = DateTime.Now;
            await UserManager.UpdateAsync(user);
        }

     
        public override async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor) 
        {
            var result = await base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByLoginAsync(loginProvider, providerKey);
                user.LastLoginDate = DateTime.Now;
                await UserManager.UpdateAsync(user);
            }
            return result;
        }


        public override async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent) 
        {
            var result = await base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByLoginAsync(loginProvider, providerKey);
                user.LastLoginDate = DateTime.Now;
                await UserManager.UpdateAsync(user);
            }
            return result;
        }
    }
}