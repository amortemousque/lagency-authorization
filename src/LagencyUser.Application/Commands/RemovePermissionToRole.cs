using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class RemovePermissionToRoleCommand : IRequest<bool>
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
                    