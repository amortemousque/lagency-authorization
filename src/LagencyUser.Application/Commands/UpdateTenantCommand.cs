using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class UpdateTenantCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public string LogoUri { get; set; }
    }
}
    