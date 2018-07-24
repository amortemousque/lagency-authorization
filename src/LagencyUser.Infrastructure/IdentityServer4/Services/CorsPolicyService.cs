﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.MongoDB.DbContexts;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace LagencyUserInfrastructure.IdentityServer4.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly DbContext _context;
        private readonly ILogger<CorsPolicyService> _logger;


        public CorsPolicyService(DbContext context, ILogger<CorsPolicyService> logger)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            
            _context = context;
            _logger = logger;
        }

        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            // If we use SelectMany directly, we got a NotSupportedException inside MongoDB driver.
            // Details: 
            // System.NotSupportedException: Unable to determine the serialization information for the collection 
            // selector in the tree: aggregate([]).SelectMany(x => x.AllowedCorsOrigins.Select(y => y.Origin))
            var origins = _context.Clients.AsQueryable().Select(x => x.AllowedCorsOrigins.Select(y => y.Origin)).ToList();

            // As a workaround, we use SelectMany in memory.
            var distinctOrigins = origins.SelectMany(o => o).Where(x => x != null).Distinct();

            var isAllowed = distinctOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            _logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, isAllowed);

            return Task.FromResult(isAllowed);
        }
    }
}