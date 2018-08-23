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
    public class UsersController : Controller
    {
        protected IMediator _mediator;

        private readonly UserManager<Model.IdentityUser> _userManager;
        private readonly SignInManager<Model.IdentityUser> _signInManager;
        private readonly UserQueries _userQueries;

        public UsersController(
            IMediator mediator,
            UserQueries userQueries,
            UserManager<Model.IdentityUser> userManager,
            SignInManager<Model.IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
            _userQueries = userQueries;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Model.IdentityUser), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                    throw new KeyNotFoundException();
                
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Model.IdentityUser[]), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> GetUsers(string name, string email)
        {
            try
            {
                var users = await _userQueries.GetUsersAsync(name, email);
                return Ok(users);
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
        [ProducesResponseType(typeof(Model.IdentityUser), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Post([FromBody]CreateUserCommand command)
        {
            try
            {
                var user = await _mediator.Send(command);
                return Ok(user);
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
        public async Task<IActionResult> Put([FromBody] UpdateUserCommand command)
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
        public async Task<IActionResult> Delete(DeleteUserCommand command)
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
