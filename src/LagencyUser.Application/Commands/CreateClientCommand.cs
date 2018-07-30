using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class CreateClientCommand : IRequest<Client>
    {
        public int ClientTypeId { get; set; }

        public string ClientName { get; set; }
    }
}
        