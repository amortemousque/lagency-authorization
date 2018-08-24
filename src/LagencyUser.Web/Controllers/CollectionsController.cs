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
using Microsoft.Extensions.Localization;
using LagencyUser.Web.Resources;

namespace LagencyUser.Web.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class CollectionsController : Controller
    {
        
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CollectionsController(
            IStringLocalizer<SharedResource> localizer)
        {
            this._localizer = localizer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [Route("ClientTypes")]
        public async Task<IActionResult> GetClientTypes()
        {
            var types = Enumeration.GetAll<ClientType>().ToList();
            types.ForEach(industry => industry.Name = _localizer[industry.Name]);
            return Ok(types);
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [Route("AllowedScopes")]
        public async Task<IActionResult> GetAllowedScopes()
        {
            var scopes = Enumeration.GetAll<AllowedScope>().ToList();
            return Ok(scopes);
        }
    }
}
