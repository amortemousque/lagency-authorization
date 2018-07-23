using IdentityServer4.MongoDB.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace IdentityServer4.MongoDB.DbContexts
{
    public class MongoDBContextBase : IDisposable
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;
        
        public MongoDBContextBase(IOptions<MongoDBConfiguration> settings)
        {
            if (settings.Value == null)
                throw new ArgumentNullException(nameof(settings), "MongoDBConfiguration cannot be null.");

            if (settings.Value.DefaultConnection == null)
                throw new ArgumentNullException(nameof(settings), "MongoDBConfiguration.ConnectionString cannot be null.");

            var databaseName = MongoUrl.Create(settings.Value.DefaultConnection).DatabaseName;

            _client = new MongoClient(settings.Value.DefaultConnection);
            _database = _client.GetDatabase(databaseName);
        }

        protected IMongoDatabase Database { get { return _database; } }

        public void Dispose()
        { 
            // TODO
        }
    }
}
