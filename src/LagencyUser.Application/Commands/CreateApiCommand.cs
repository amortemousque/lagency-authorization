using System;
using System.Collections.Generic;
using LagencyUser.Application.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class CreateApiCommand : IRequest<ApiResource>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
    