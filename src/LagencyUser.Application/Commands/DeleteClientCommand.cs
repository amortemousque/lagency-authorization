using System;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class DeleteClientCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
