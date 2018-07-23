// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.MongoDB;
using IdentityServer4.MongoDB.DbContexts;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using LagencyUserInfrastructure.IdentityServer4.Services;
using LagencyUserInfrastructure.IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LagencyUserInfrastructure.IdentityServer4.Extensions
{
    public static class IdentityServerMongoDBBuilderExtensions
    {
        public static IIdentityServerBuilder AddMongoDbConfigurationStore(
            this IIdentityServerBuilder builder, string connectionString)
        {
            
            builder.Services.AddScoped<ConfigurationDbContext>(cw => new ConfigurationDbContext(connectionString));

            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IResourceStore, ResourceStore>();
            builder.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();


            //builder.Services.Configure<string>(configuration);
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            var tokenCleanupOptions = new TokenCleanupOptions();
            builder.Services.AddSingleton(tokenCleanupOptions);
            builder.Services.AddSingleton<TokenCleanup>();

            return builder;
        }


        public static IApplicationBuilder UseIdentityServerMongoDBTokenCleanup(this IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            var tokenCleanup = app.ApplicationServices.GetService<TokenCleanup>();
            if (tokenCleanup == null)
            {
                throw new InvalidOperationException("AddOperationalStore must be called on the service collection.");
            }
            applicationLifetime.ApplicationStarted.Register(tokenCleanup.Start);
            applicationLifetime.ApplicationStopping.Register(tokenCleanup.Stop);

            return app;
        }
    }
}