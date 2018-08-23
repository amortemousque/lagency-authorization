using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class CreateTenantCommand : IRequest<Tenant>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
        