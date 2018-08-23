using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LagencyUser.Web.Services;
using LagencyUser.Web.Models;
using LagencyUser.Web.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MediatR;
using LagencyUser.Application.Queries;
using System;
using LagencyUser.Application.Model;
using System.Collections.Generic;
using LagencyUser.Application.Commands;

namespace LagencyUser.Web.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class TenantsController : Controller
    {
        protected IMediator _mediator;
        private readonly TenantQueries _tenantQueries;

        public TenantsController(
            IMediator mediator,
            TenantQueries tenantQueries)
        {
            _mediator = mediator;
            _tenantQueries = tenantQueries;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Tenant), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetTenant(Guid id)
        {
            try
            {
                var api = await _tenantQueries.GetTenantByIdAsync(id);
                return Ok(api);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Tenant[]), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> GetTenants(string name, bool? enabled)
        {
            try
            {
                var tenants = await _tenantQueries.GetTenantsAsync(name, enabled);
                return Ok(tenants);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(argumentException.Message);
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(Tenant), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Post([FromBody]CreateTenantCommand command)
        {
            try
            {
                var tenant = await _mediator.Send(command);
                return Ok(tenant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(argumentException.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Put([FromBody] UpdateTenantCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(argumentException.Message);
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Delete(DeleteTenantCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(argumentException.Message);
            }

        }
    }
}
