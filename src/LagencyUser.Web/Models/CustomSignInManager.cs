
using System;
using System.Threading.Tasks;
using LagencyUser.Application.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using Model = LagencyUser.Application.Model;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Collections.Generic;

namespace LagencyUser.Web
{
    public class CustomSignInManager : SignInManager<Model.IdentityUser>
    {
        //public CustomSignInManager(UserManager<Model.IdentityUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<Model.IdentityUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<Model.IdentityUser>> logger, IAuthenticationSchemeProvider schemes)
        //    : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        //{
        //}


        private readonly UserManager<Model.IdentityUser> _userManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomSignInManager(UserManager<Model.IdentityUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<Model.IdentityUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<Model.IdentityUser>> logger, IAuthenticationSchemeProvider schemes, IRoleRepository roleRepository)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));

            if (roleRepository == null)
                throw new ArgumentNullException(nameof(roleRepository));

            if (contextAccessor == null)
                throw new ArgumentNullException(nameof(contextAccessor));

            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _roleRepository = roleRepository;
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            var user = await _userManager.FindByNameAsync(userName);
            await PermissionsSynchro(user);
            var result = await base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
            if(result.Succeeded) {
                user.LastLoginDate = DateTime.Now;
                await UserManager.UpdateAsync(user);
            }
            return result;
        }

        public override async Task SignInAsync(Model.IdentityUser user, bool isPersistent, string authenticationMethod = null)
        {
            await PermissionsSynchro(user);
            await base.SignInAsync(user, isPersistent, authenticationMethod);
            user.LastLoginDate = DateTime.Now;
            await UserManager.UpdateAsync(user);
        }

        public override async Task SignInAsync(Model.IdentityUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null) 
        {
            await PermissionsSynchro(user);
            await base.SignInAsync(user, authenticationProperties, authenticationMethod);
            user.LastLoginDate = DateTime.Now;
            await UserManager.UpdateAsync(user);
        }

     
        public override async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor) 
        {
            var user = await UserManager.FindByLoginAsync(loginProvider, providerKey);
            await PermissionsSynchro(user);
            var result = await base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
            if (result.Succeeded)
            {
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



        //public override async Task<SignInResult> PasswordSignInAsync(Model.IdentityUser user, string password, bool isPersistent, bool lockoutOnFailure)
        //{
        //    await PermissionsSynchro(user);
        //    return await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        //}

        //public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        //{
        //    var user = await _userManager.FindByNameAsync(userName);
        //    await PermissionsSynchro(user);
        //    return await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        //}


        private async Task<bool> PermissionsSynchro(Model.IdentityUser user)
        {
            if(user != null) {
                var claims = await _userManager.GetClaimsAsync(user);
                var claimRole = claims.FirstOrDefault(c => c.Type == "role");
                var claimPermission = claims.FirstOrDefault(c => c.Type == "permission");

                var roles = JsonConvert.DeserializeObject<List<string>>(claimRole.Value);
                var permissions = await _roleRepository.GetRolePermissions(roles.ToArray());
                if(claimPermission != null) 
                {
                    await _userManager.ReplaceClaimAsync(user, claimPermission, new Claim("permission", JsonConvert.SerializeObject(permissions)));
                } 
                else 
                {
                    await _userManager.AddClaimAsync(user, new Claim("permission", JsonConvert.SerializeObject(permissions)));
                }

                return true;
            }
    
            return false;
        }
    }
}