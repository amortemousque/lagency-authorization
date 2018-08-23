using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class CreateApiScopeCommand : IRequest<ApiScope>
    {
        public Guid Id { get; set; }
        public Guid ApiResourceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
