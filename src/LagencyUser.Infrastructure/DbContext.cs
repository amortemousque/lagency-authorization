// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using LagencyUserApplication.Model;
using LagencyUserInfrastructure.IdentityServer4;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.MongoDB.DbContexts
{
    public class ConfigurationDbContext
    {

        private readonly IMongoDatabase _database; 
        private readonly IMongoClient _client; 

        private IMongoCollection<Client> _clients;
        private IMongoCollection<IdentityResource> _identityResources;
        private IMongoCollection<ApiResource> _apiResources;

        public ConfigurationDbContext(string connectionString)
        {
            if (connectionString == null) 
                throw new ArgumentNullException(nameof(connectionString), "MongoDBConfiguration cannot be null."); 
 
           
            var mongoDbMementoDatabaseName = MongoUrl.Create(connectionString).DatabaseName;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName; 
 
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName); 


            _clients = _database.GetCollection<Client>(Constants.TableNames.Client);
            _identityResources = _database.GetCollection<IdentityResource>(Constants.TableNames.IdentityResource);
            _apiResources = _database.GetCollection<ApiResource>(Constants.TableNames.ApiResource);
        }

        public IQueryable<Client> Clients
        {
            get { return _clients.AsQueryable(); }
        }


        public IQueryable<IdentityResource> IdentityResources
        {
            get { return _identityResources.AsQueryable(); }
        }

        public IQueryable<ApiResource> ApiResources
        {
            get { return _apiResources.AsQueryable(); }
        }

        public async Task AddClient(Client entity)
        {
            await _clients.InsertOneAsync(entity);
        }

        public async Task AddIdentityResource(IdentityResource entity)
        {
            await _identityResources.InsertOneAsync(entity);
        }

        public async Task AddApiResource(ApiResource entity)
        {
            await _apiResources.InsertOneAsync(entity);
        }

    }
}    