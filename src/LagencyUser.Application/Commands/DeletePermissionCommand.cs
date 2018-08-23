using System;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class DeletePermissionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
        