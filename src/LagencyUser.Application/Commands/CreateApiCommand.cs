using System;
using LagencyUserApplication.Model;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class CreateApiCommand : IRequest<ApiResource>
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
    