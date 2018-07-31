using System.Linq;
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
using LagencyUser.Application.Model;
using System.Collections.Generic;
using LagencyUser.Application.Commands;

namespace IdentityServerWithAspNetIdentity.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class ApisController : Controller
    {
        protected IMediator _mediator;
        private readonly ApiQueries _apiQueries;

        public ApisController(
            IMediator mediator,
            ApiQueries apiQueries)
        {
            _mediator = mediator;
            _apiQueries = apiQueries;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResource), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetApi(Guid id)
        {
            try
            {
                var api = await _apiQueries.GetApiAsync(id);
                return Ok(api);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResource[]), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> GetApis(string name, string displayName, bool? enabled)
        {
            var apis = await _apiQueries.GetApisAsync(name, displayName, enabled);
            return Ok(apis); 
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResource), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Post([FromBody]CreateApiCommand command)
        {
            var apiResource = await _mediator.Send(command);
            return Ok(apiResource);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Put([FromBody]UpdateApiCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Delete(DeleteApiCommand command)
        {
            var client = await _mediator.Send(command);
            return Ok();
        }


        //scope

        [HttpGet("{apiResourceId}/scopes", Name = "GetApiScopes")]
        [ProducesResponseType(typeof(ApiResource[]), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> GetApiScopes(Guid apiResourceId)
        {
            var scopes = await _apiQueries.GetApiScopesAsync(apiResourceId);
            return Ok(scopes);
        }


        [HttpPost("{apiResourceId}/scopes", Name = "PostApiScope")]
        [ProducesResponseType(typeof(ApiResource), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> PostApiScope([FromBody]CreateApiScopeCommand command)
        {
            var apiResource = await _mediator.Send(command);
            return Ok(apiResource);
        }


        [HttpPut("{apiResourceId}/scopes/{id}", Name = "PutApiScope")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> PutApiScope([FromBody]UpdateApiScopeCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{apiResourceId}/scopes/{id}", Name = "DeleteApiScope")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> DeleteApiScope(DeleteApiScopeCommand command)
        {
            var client = await _mediator.Send(command);
            return Ok();
        }
    }
}
    