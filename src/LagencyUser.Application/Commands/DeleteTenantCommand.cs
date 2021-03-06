﻿using System;
using MediatR;

namespace LagencyUser.Application.Commands
{
    public class DeleteTenantCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
    