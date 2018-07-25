﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServerWithAspNetIdentity.Services;
using IdentityServerWithAspNetIdentity.Models;
using IdentityServerWithAspNetIdentity.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MediatR;
using LagencyUser.Application.Queries;
using System;
using LagencyUserApplication.Model;
using System.Collections.Generic;
using LagencyUser.Application.Commands;

namespace IdentityServerWithAspNetIdentity.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class ClientsController : Controller
    {
        protected IMediator _mediator;
        private readonly ClientQueries _clientQueries;

        public ClientsController(
            IMediator mediator,
            ClientQueries clientQueries)
        {
            _mediator = mediator;
            _clientQueries = clientQueries;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Client), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetClient(Guid id)
        {
            try
            {
                var api = await _clientQueries.GetClientAsync(id);
                return Ok(api);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Client[]), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> GetClients(string name, bool? enabled)
        {
            var clients = await _clientQueries.GetClientsAsync(name, enabled);
            return Ok(clients);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Client), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Post([FromBody]CreateClientCommand command)
        {
            var client = await _mediator.Send(command);
            return Ok(client);
        }

    }
}
