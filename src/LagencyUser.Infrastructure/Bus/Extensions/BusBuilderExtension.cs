using System;
using LagencyUser.Application.Model;
using LagencyUser.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Rebus.ServiceProvider;
using Rebus.Routing.TypeBased;
using Rebus.Config;
using Rebus.RabbitMq;
using LagencyUser.Application.CommandHandlers;
using LagencyUser.Application.Events;
using Rebus.Serialization.Json;
using Newtonsoft.Json;
using IntegrationEvents;

namespace LagencyUser.Infrastructure.Bus.Extensions
{


    public static class BusBuilderExtensions
    {
       

        /// <summary>
        ///     This method allows you to customize the user and role type when registering identity services
        ///     and MongoDB stores.
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TRole"></typeparam>
        /// <param name="services"></param>
        /// <param name="connectionString">Connection string must contain the database name</param>
        public static IServiceCollection AddRebusConfiguration(this IServiceCollection services, string connectionString, string queueName)
        {
            // Register handlers 
            services.AutoRegisterHandlersFromAssemblyOf<ApiHandlers>();

            var jsonConfiguration = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
            // Configure and register Rebus
            return services.AddRebus(configure => configure
                .Serialization(s => s.UseNewtonsoftJson(jsonConfiguration))
                .Logging(l => l.Console())
                .Transport(t => 
                           t.UseRabbitMq(connectionString, queueName))
                .Routing(r => r.TypeBased()
                         //.Map<SendEmail>("LagencyNotification.Application.Events.SendEmail")
                         .MapAssemblyOf<SendEmail>(queueName)));
        }
    }
}
