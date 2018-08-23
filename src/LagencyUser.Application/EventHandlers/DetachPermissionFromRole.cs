using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LagencyUser.Application.Contracts;
using LagencyUser.Application.Events;
using LagencyUser.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LagencyUser.Application.EventHandlers
{
    public class DetachPermissionFromRole : INotificationHandler<PermissionDeletedEvent>
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPermissionRepository _permissionRepository;

        public DetachPermissionFromRole(RoleManager<IdentityRole> roleManager,
                                      IPermissionRepository permissionRepository)
        {
            _roleManager = roleManager;
            _permissionRepository = permissionRepository;
        }

        public async Task Handle(PermissionDeletedEvent notification, CancellationToken cancellationToken)
        {
            //Delete permission reference into roles
            var roles = _roleManager.Roles.ToList().Where(role => role.Permissions.Any(pname => pname == notification.Name));

            foreach(var role in roles) 
            {
                role.Permissions.Remove(notification.Name);
                await _roleManager.UpdateAsync(role);
            }
        }
    }
}
        