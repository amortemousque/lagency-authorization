﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using LagencyUserInfrastructure.IdentityServer4.Mappers;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using LagencyUserInfrastructure.Context;

namespace LagencyUserInfrastructure.IdentityServer4.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PersistedGrantStore> _logger;


        public PersistedGrantStore(ApplicationDbContext context, ILogger<PersistedGrantStore> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task StoreAsync(PersistedGrant token)
        {
            try
            {
                var existing = _context.PersistedGrants.AsQueryable().SingleOrDefault(x => x.Key == token.Key);
                if (existing == null)
                {
                    _logger.LogDebug("{persistedGrantKey} not found in database", token.Key);

                    var persistedGrant = token.ToEntity();
                    _context.PersistedGrants.InsertOneAsync(persistedGrant);
                }
                else
                {
                    _logger.LogDebug("{persistedGrantKey} found in database", token.Key);

                    token.UpdateEntity(existing);
                    _context.PersistedGrants.ReplaceOneAsync(x => x.Key == token.Key, existing);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Exception storing persisted grant");
            }

            return Task.FromResult(0);
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = _context.PersistedGrants.AsQueryable().FirstOrDefault(x => x.Key == key);
            var model = persistedGrant.ToModel();

            _logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);

            return Task.FromResult(model);
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var persistedGrants = _context.PersistedGrants.AsQueryable().Where(x => x.SubjectId == subjectId).ToList();
            var model = persistedGrants.Select(x => x.ToModel());

            _logger.LogDebug("{persistedGrantCount} persisted grants found for {subjectId}", persistedGrants.Count, subjectId);

            return Task.FromResult(model);
        }

        public Task RemoveAsync(string key)
        {
            _logger.LogDebug("removing {persistedGrantKey} persisted grant from database", key);

            _context.PersistedGrants.DeleteManyAsync(x => x.Key == key);
            
            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            _logger.LogDebug("removing persisted grants from database for subject {subjectId}, clientId {clientId}", subjectId, clientId);

            _context.PersistedGrants.DeleteManyAsync(x => x.SubjectId == subjectId && x.ClientId == clientId);

            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            _logger.LogDebug("removing persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {persistedGrantType}", subjectId, clientId, type);

            _context.PersistedGrants.DeleteManyAsync(
               x =>
               x.SubjectId == subjectId &&
               x.ClientId == clientId &&
               x.Type == type);

            return Task.FromResult(0);
        }
    }
}