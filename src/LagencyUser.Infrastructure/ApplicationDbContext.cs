// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using LagencyUser.Application.Model;
using LagencyUser.Infrastructure.IdentityServer4;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LagencyUser.Infrastructure.Context
{
    public class ApplicationDbContext : IDisposable
    {
        private readonly IMongoDatabase _database; 
        private readonly IMongoClient _client; 

        public IMongoCollection<Client> Clients
        {
            get
            {
                return _database.GetCollection<Client>(Constants.TableNames.Client);
            }
        }

        public IMongoCollection<IdentityResource> IdentityResources
        {
            get
            {
                return _database.GetCollection<IdentityResource>(Constants.TableNames.IdentityResource);
            }
        }


        public IMongoCollection<ApiResource> ApiResources
        {
            get
            {
                return _database.GetCollection<ApiResource>(Constants.TableNames.ApiResource);
            }
        }

        public IMongoCollection<PersistedGrant> PersistedGrants
        {
            get
            {
                return _database.GetCollection<PersistedGrant>(Constants.TableNames.PersistedGrant);
            }
        }

        public ApplicationDbContext(string connectionString)
        {
            if (connectionString == null) 
                throw new ArgumentNullException(nameof(connectionString), "MongoDBConfiguration cannot be null."); 
 
            var mongoDbMementoDatabaseName = MongoUrl.Create(connectionString).DatabaseName;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName; 
 
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public async Task AddClient(Client entity)
        {
            await Clients.InsertOneAsync(entity);
        }

        public async Task AddIdentityResource(IdentityResource entity)
        {
            await IdentityResources.InsertOneAsync(entity);
        }

        public async Task AddApiResource(ApiResource entity)
        {
            await ApiResources.InsertOneAsync(entity);
        }

        public void Dispose()
        {
            //Todo
        }
    }
}    