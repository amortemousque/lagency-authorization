using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class AddPermissionsToRoleCommand : IRequest<bool>
    {
        public Guid RoleId { get; set; }
        public string[] Permissions { get; set; }
    }
}
                