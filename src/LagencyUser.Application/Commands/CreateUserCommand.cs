using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class CreateUserCommand : IRequest<IdentityUser>
    {
        public Guid TenantId { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Culture { get; set; }
        public List<string> Roles { get; set; }
        public string ProviderName { get; set; }
        public string ProviderData { get; set; }
    }
}
            