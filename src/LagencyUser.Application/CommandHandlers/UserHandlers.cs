using System.Threading.Tasks;
using MediatR;
using System;
using System.Threading;
using LagencyUser.Application.Commands;
using LagencyUser.Application.Contracts;
using System.Linq;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using IModels = IdentityServer4.Models;
using LagencyUser.Application.Service;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityModel;
using Newtonsoft.Json;
using Rebus.Bus;
using LagencyUser.Application.Events;
using IntegrationMessages.Commands;

namespace LagencyUser.Application.CommandHandlers
{
    public class UserHandlers :
    IRequestHandler<CreateUserCommand, IdentityUser>,
    IRequestHandler<UpdateUserCommand, bool>,
    IRequestHandler<DeleteUserCommand, bool>

    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        private readonly IBus _bus;

        public UserHandlers(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityUser> Handle(CreateUserCommand message, CancellationToken cancellationToken)
        {
            var user = new Model.IdentityUser { 
                TenantId = message.TenantId,
                UserName = message.Email, 
                Email = message.Email, 
                GivenName = message.GivenName, 
                FamilyName = message.FamilyName 
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                if (result.Errors.First().Code == "DuplicateUserName") 
                {
                    throw new ArgumentException(result.Errors.First().Description, "email");
                }
            }

            await _userManager.AddToRolesAsync(user, message.Roles.Where(r => user.Roles.Any(ur => r != ur)));


            result = _userManager.AddClaimsAsync(user, new Claim[]{
                        new Claim(JwtClaimTypes.Name, user.Email),
                        new Claim(JwtClaimTypes.GivenName, user.GivenName),
                        new Claim(JwtClaimTypes.FamilyName, user.FamilyName),

                        new Claim(JwtClaimTypes.Email,user.Email),
                        new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed.ToString(), ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.Role, JsonConvert.SerializeObject(user.Roles), IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                //new Claim(JwtClaimTypes.Role, JsonConvert.SerializeObject(user.), IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),

                        //new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        //new Claim("location", "somewhere")
                    }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            return user;
        }

        public async Task<bool> Handle(UpdateUserCommand message, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(message.Id.ToString());
            user.GivenName = message.GivenName;
            user.FamilyName = message.FamilyName;

            var email = await _userManager.GetEmailAsync(user);

            if (message.Email.ToLower() != email.ToLower()) {
                await _userManager.SetUserNameAsync(user, email);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ChangeEmailAsync(user, message.Email, code);
            }
       
            await _userManager.UpdateAsync(user);

            var rolesToDelete = user.Roles.Where(ur => !message.Roles.Any(r => r == ur));
            var result = await _userManager.RemoveFromRolesAsync(user, rolesToDelete);
            var rolesToAdd = message.Roles.Where(r => !user.Roles.Any(ur => r == ur)).ToList();
            var result2 = await _userManager.AddToRolesAsync(user, rolesToAdd);

            return true;
        }

        public async Task<bool> Handle(DeleteUserCommand message, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(message.Id.ToString());

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new ArgumentException(result.Errors.First().Description, nameof(user));
            }

            return true;

        }
    }
}            