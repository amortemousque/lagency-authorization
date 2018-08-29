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
using IntegrationEvents;

namespace LagencyUser.Application.CommandHandlers
{
    public class UserHandlers :
    IRequestHandler<CreateUserCommand, IdentityUser>,
    IRequestHandler<UpdateUserCommand, bool>,
    IRequestHandler<DeleteUserCommand, bool>

    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly ITenantRepository _repository;

        private readonly IBus _bus;

        public UserHandlers(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ITenantRepository repository,
            IBus bus
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
            _bus = bus;
        }

        public async Task<IdentityUser> Handle(CreateUserCommand message, CancellationToken cancellationToken)
        {
            if (message.TenantId == null)
                throw new ArgumentException("An email must be specified", nameof(message.TenantId));

            var tenant = await _repository.GetById(message.TenantId) ?? throw new ArgumentException("The tenant does not exist", nameof(message.TenantId));

            var user = new Model.IdentityUser {
                TenantId = message.TenantId,
                UserName = message.Email,
                Email = message.Email, 
                GivenName = message.GivenName, 
                FamilyName = message.FamilyName,
                Culture = message.Culture,
                Picture = message.Picture,
                LinkedinProviderData = message.ProviderName != null && message.ProviderName.ToLower() == "linkedin" ? message.ProviderData : null,
                GoogleProviderData = message.ProviderName != null &&  message.ProviderName.ToLower() == "google" ? message.ProviderData : null
            };

            IdentityResult result = string.IsNullOrWhiteSpace(message.Password) ? await _userManager.CreateAsync(user) : await _userManager.CreateAsync(user, message.Password);

            if (!result.Succeeded)
            {
                if (result.Errors.First().Code == "DuplicateUserName") 
                {
                    throw new ArgumentException(result.Errors.First().Description, "email");
                }
                else 
                {
                    throw new ArgumentException(result.Errors.First().Description);
                }
            }

            if (message.Roles != null) 
            {
                result = await _userManager.AddToRolesAsync(user, message.Roles.Where(r => user.Roles.Any(ur => r != ur)));
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }


            result = await _userManager.AddClaimsAsync(user, new Claim[]{
                new Claim(JwtClaimTypes.Name, user.Email ?? ""),
                new Claim(JwtClaimTypes.GivenName, user.GivenName ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.FamilyName ?? ""),
                new Claim(JwtClaimTypes.Email, user.Email ?? ""),
                new Claim(JwtClaimTypes.Picture, user.Picture ?? ""),
                new Claim(JwtClaimTypes.Locale, user.Culture ?? ""),
                new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed.ToString(), ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.Role, JsonConvert.SerializeObject(user.Roles), IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                new Claim("tenant_id", tenant.Id.ToString()),
                new Claim("tenant_name", tenant.Name)
            });

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            var ev = new UserCreatedEvent
            {
                UserId = Guid.Parse(user.Id),
                TenantId = user.TenantId,
                GivenName = user.GivenName,
                FamilyName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Picture = user.Picture,
                ProviderName = message.ProviderName,
                ProviderData = message.ProviderData,
                RegistrationDate = user.RegistrationDate
            };
            await _bus.Send(ev);

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

            if (message.EmailConfirmed != null) {
                user.EmailConfirmed = message.EmailConfirmed.Value;
            }
       
            await _userManager.UpdateAsync(user);

            var rolesToDelete = user.Roles.Where(ur => !message.Roles.Any(r => r == ur)).ToList();
            var result = await _userManager.RemoveFromRolesAsync(user, rolesToDelete);
            var rolesToAdd = message.Roles.Where(r => !user.Roles.Any(ur => r == ur)).ToList();
            var result2 = await _userManager.AddToRolesAsync(user, rolesToAdd);

            var claims = user.Claims.Select(cw => cw.ToSecurityClaim()).ToList();
            result = await _userManager.RemoveClaimsAsync(user, claims);
            var tenant = await _repository.GetById(user.TenantId);

            result = await _userManager.AddClaimsAsync(user, new Claim[]{
                new Claim(JwtClaimTypes.Name, user.Email ?? ""),
                new Claim(JwtClaimTypes.GivenName, user.GivenName ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.FamilyName ?? ""),
                new Claim(JwtClaimTypes.Locale, user.Culture ?? ""),
                new Claim(JwtClaimTypes.Picture, user.Picture ?? ""),
                new Claim(JwtClaimTypes.Email, user.Email ?? ""),
                new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed.ToString(), ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.Role, JsonConvert.SerializeObject(user.Roles), IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                new Claim("tenant_id", tenant.Id.ToString()),
                new Claim("tenant_name", tenant.Name)
            });


        
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