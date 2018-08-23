using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class UpdateApiScopeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid ApiResourceId { get; set; }
        public string Description { get; set; }
    }
}
