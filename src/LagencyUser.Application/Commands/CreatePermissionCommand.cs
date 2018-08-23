using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class CreatePermissionCommand : IRequest<Permission>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
            