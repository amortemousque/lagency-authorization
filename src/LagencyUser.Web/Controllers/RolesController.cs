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
using Model = LagencyUser.Application.Model;

namespace LagencyUser.Web.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class RolesController : Controller
    {
        protected IMediator _mediator;

        private readonly RoleManager<Model.IdentityRole> _roleManager;
        private readonly RoleQueries _roleQueries;

        public RolesController(
            IMediator mediator,
            RoleQueries roleQueries,
            RoleManager<Model.IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _mediator = mediator;
            _roleQueries = roleQueries;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Model.IdentityRole), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetRole(Guid id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                    throw new KeyNotFoundException();

                return Ok(role);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Model.IdentityRole[]), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> GetRoles(string name)
        {
            try
            {
                var roles = await _roleQueries.GetRolesAsync(name);
                return Ok(roles);
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
        [ProducesResponseType(typeof(Model.IdentityRole), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Post([FromBody]CreateRoleCommand command)
        {
            try
            {
                var role = await _mediator.Send(command);
                return Ok(role);
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
        public async Task<IActionResult> Put([FromBody] UpdateRoleCommand command)
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
        public async Task<IActionResult> Delete(DeleteRoleCommand command)
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



        // permission

        [HttpGet("{roleId}/permissions", Name = "GetRolePermissions")]
        [ProducesResponseType(typeof(Permission[]), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> GetRolePermissions(Guid roleId)
        {
            try
            {
                var permissions = await _roleQueries.GetRolePermissionsAsync(roleId);
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


        [HttpPost("{roleId}/permissions", Name = "PostPermissionRole")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> PostPermissionRole([FromBody]AddPermissionsToRoleCommand command)
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


        [HttpDelete("{roleId}/permissions/{permissionId}", Name = "DeletePermissionRole")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> DeletePermissionRole(RemovePermissionToRoleCommand command)
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
