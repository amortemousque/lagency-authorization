using System;
using System.Collections.Generic;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class UpdateApiCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<string> UserClaims { get; set; } 
    }
}
    