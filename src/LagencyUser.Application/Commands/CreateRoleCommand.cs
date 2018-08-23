using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class CreateRoleCommand : IRequest<IdentityRole>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }

        public List<string> Permissions { get; set; }

    }
}
        