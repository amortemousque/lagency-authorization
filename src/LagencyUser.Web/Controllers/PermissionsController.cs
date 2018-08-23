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
    public class PermissionsController : Controller
    {
        protected IMediator _mediator;
        private readonly PermissionQueries _permissionQueries;

        public PermissionsController(
            IMediator mediator,
            PermissionQueries permissionQueries)
        {
            _mediator = mediator;
            _permissionQueries = permissionQueries;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Permission), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetPermission(Guid id)
        {
            try
            {
                var api = await _permissionQueries.GetPermissionAsync(id);
                return Ok(api);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Permission[]), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> GetPermissions(string name)
        {
            try
            {
                var permissions = await _permissionQueries.GetPermissionsAsync(name);
                return Ok(permissions);
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
        [ProducesResponseType(typeof(Permission), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Post([FromBody]CreatePermissionCommand command)
        {
            try
            {
                var permission = await _mediator.Send(command);
                return Ok(permission);
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
        public async Task<IActionResult> Put([FromBody] UpdatePermissionCommand command)
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
        public async Task<IActionResult> Delete(DeletePermissionCommand command)
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

        [HttpDelete( Name = "DeleteMultiple")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> DeleteMultiple(Guid[] ids)
        {
            try
            {
                foreach(var id in ids) 
                {
                    var command = new DeletePermissionCommand() { Id = id };
                    await _mediator.Send(command);
                }
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
