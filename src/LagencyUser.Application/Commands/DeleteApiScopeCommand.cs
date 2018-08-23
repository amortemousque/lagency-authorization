using System;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class DeleteApiScopeCommand : IRequest<bool>
    {
        public Guid ApiResourceId { get; set; }
        public Guid Id { get; set; }
    }
}
    