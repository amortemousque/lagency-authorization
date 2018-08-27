using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public List<string> Roles { get; set; }
    }
}
        