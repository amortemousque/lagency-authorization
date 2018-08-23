using System;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class DeleteApiCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
    