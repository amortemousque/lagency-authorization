﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.MongoDB.DbContexts;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using LagencyUserInfrastructure.IdentityServer4.Mappers;
using MongoDB.Driver;
using LagencyUserInfrastructure.Context;

namespace LagencyUserInfrastructure.IdentityServer4.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly DbContext _context;
        private readonly ILogger<ClientStore> _logger;

        public ClientStore(DbContext context, ILogger<ClientStore> logger)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            
            _context = context;
            _logger = logger;
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _context.Clients.AsQueryable().FirstOrDefault(x => x.ClientId == clientId);

            var model = client?.ToModel();

            _logger.LogDebug("{clientId} found in database: {clientIdFound}", clientId, model != null);

            return Task.FromResult(model);
        }
    }
}