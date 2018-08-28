﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LagencyUser.Web.Models;
using LagencyUser.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using IdentityServer4;
using LagencyUser.Web.Services;
using LagencyUser.Infrastructure.Identity.Extensions;
using LagencyUser.Infrastructure.IdentityServer4.Extensions;
using LagencyUser.Application.Contracts;
using LagencyUser.Infrastructure.Repositories;
using LagencyUser.Application.CommandHandlers;
using MediatR;
using System.Reflection;
using LagencyUser.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using LagencyUser.Infrastructure.Context;
using Model = LagencyUser.Application.Model;
using LagencyUser.Infrastructure.Message;
using LagencyUser.Infrastructure.Bus.Extensions;
using LagencyUser.Web.Extensions;
using Rebus.Bus;
using Microsoft.AspNetCore.Mvc.Razor;
using LagencyUser.Web.Resources;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.HttpOverrides;

namespace LagencyUser.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<SignInManager<Model.IdentityUser>, CustomSignInManager>();

            //Application - Db Context
            var context = new ApplicationDbContext(Configuration.GetRequiredValue<string>("MONGO_URL"));
            services.AddScoped<ApplicationDbContext>(cw => context);


            // Application - Repositories
            services.AddScoped<IApiResourceRepository, ApiResourceRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IIdentityResourceRepository, IdentityResourceRepository>();
            services.AddScoped<IPersistedGrandRepository, PersistedGrantRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            // Application - Commands
            services.AddMediatR(typeof(ApiHandlers).GetTypeInfo().Assembly);


            // Application - Queries
            services.AddScoped<ApiQueries, ApiQueries>();
            services.AddScoped<ClientQueries, ClientQueries>();
            services.AddScoped<TenantQueries, TenantQueries>();
            services.AddScoped<UserQueries, UserQueries>();
            services.AddScoped<RoleQueries, RoleQueries>();
            services.AddScoped<PermissionQueries, PermissionQueries>();

            //Custom extension for mongo implementation
            //services.AddIdentityWithMongoStores(Configuration.GetConnectionString("DefaultConnection"));
            services.AddIdentityWithMongoStoresUsingCustomTypes<LagencyUser.Application.Model.IdentityUser, LagencyUser.Application.Model.IdentityRole>(context);
            services.AddScoped<IUserClaimsPrincipalFactory<LagencyUser.Application.Model.IdentityUser>, UserClaimsPrincipalFactory<LagencyUser.Application.Model.IdentityUser>>();
            services.AddScoped<UserManager<LagencyUser.Application.Model.IdentityUser>, UserManager<LagencyUser.Application.Model.IdentityUser>>();


            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization(options => {
                        options.DataAnnotationLocalizerProvider = (type, factory) =>
                            factory.Create(typeof(SharedResource));
                    });
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });


            services.AddCors();

            //Identity server

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
            })
            //Custom extension for mongo implementation       
            .AddMongoDbConfigurationStore()
            .AddDeveloperSigningCredential()
            .AddExtensionGrantValidator<LagencyUser.Infrastructure.Identity.Extensions.ExtensionGrantValidator>()
            .AddExtensionGrantValidator<LagencyUser.Infrastructure.Identity.Extensions.NoSubjectExtensionGrantValidator>()
            .AddJwtBearerClientAuthentication()
            .AddAppAuthRedirectUriValidator()
            .AddAspNetIdentity<LagencyUser.Application.Model.IdentityUser>();

            services.AddExternalIdentityProviders();


            //rebus configuration
            services.AddRebusConfiguration(Configuration.GetRequiredValue<string>("RABBITMQ_URL"), Configuration.GetRequiredValue<string>("RABBITMQ_QUEUE_NAME"));

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors(
                options => options.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
             );

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("fr")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("en")),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            var fordwardedHeaderOptions = new ForwardedHeadersOptions
           {
               ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
           };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(fordwardedHeaderOptions);

            app.UsePathBase(new PathString("/user"));
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
            app.UseRebus();

            //.Run(async (context) =>
            //{
            //    var bus = app.ApplicationServices.GetRequiredService<IBus>();

            //      //await Task.WhenAll(
            //      //Enumerable.Range(0, 10)
            //      //.Select(i => new Message1())
            //      //.Select(message => bus.Send(message)));

            //      await context.Response.WriteAsync("Rebus sent another 10 messages!");
            //});
        }


    }

    public static class ServiceExtensions
    {
        public static IServiceCollection AddExternalIdentityProviders(this IServiceCollection services)
        {
            // configures the OpenIdConnect handlers to persist the state parameter into the server-side IDistributedCache.
            services.AddOidcStateDataFormatterCache("aad", "demoidsrv");

            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    options.ClientId = "926489309778-mog8hsf0g0au6h6dt3sbkbrilit7sqqa.apps.googleusercontent.com";
                    options.ClientSecret = "gUfvfTL7ChV1JoOJHU5lRgdL";
                });
            //.AddOpenIdConnect("demoidsrv", "IdentityServer", options =>
            //{
            //options.SignInScheme = IdentityConstants.ExternalScheme;
            //    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

            //    options.Authority = "https://demo.identityserver.io/";
            //    options.ClientId = "implicit";
            //    options.ResponseType = "id_token";
            //    options.SaveTokens = true;
            //    options.CallbackPath = new PathString("/signin-idsrv");
            //    options.SignedOutCallbackPath = new PathString("/signout-callback-idsrv");
            //    options.RemoteSignOutPath = new PathString("/signout-idsrv");

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = "role"
            //    };
            //})
            //.AddOpenIdConnect("aad", "Azure AD", options =>
            //{
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

            //    options.Authority = "https://login.windows.net/4ca9cb4c-5e5f-4be9-b700-c532992a3705";
            //    options.ClientId = "96e3c53e-01cb-4244-b658-a42164cb67a9";
            //    options.ResponseType = "id_token";
            //    options.CallbackPath = new PathString("/signin-aad");
            //    options.SignedOutCallbackPath = new PathString("/signout-callback-aad");
            //    options.RemoteSignOutPath = new PathString("/signout-aad");
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = "role"
            //    };
            //})
            //.AddOpenIdConnect("adfs", "ADFS", options =>
            //{
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

            //    options.Authority = "https://adfs.leastprivilege.vm/adfs";
            //    options.ClientId = "c0ea8d99-f1e7-43b0-a100-7dee3f2e5c3c";
            //    options.ResponseType = "id_token";

            //    options.CallbackPath = new PathString("/signin-adfs");
            //    options.SignedOutCallbackPath = new PathString("/signout-callback-adfs");
            //    options.RemoteSignOutPath = new PathString("/signout-adfs");
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = "role"
            //    };
            //});

            return services;
        }
    }
}
