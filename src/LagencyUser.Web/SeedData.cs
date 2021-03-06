﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using LagencyUser.Web.Configuration;
using LagencyUser.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using LagencyUserInfrastructure.IdentityServer4.Mappers;
using LagencyUser.Infrastructure.Context;
using Model = LagencyUser.Application.Model;

namespace LagencyUser.Web
{
    public class SeedData
    {
        //public static void EnsureSeedData(IServiceProvider serviceProvider)
        //{
        //    using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //    {
        //        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<Model.IdentityUser>>();
        //        var alice = userMgr.FindByNameAsync("AliceSmith@email.com").Result;
        //        if (alice == null)
        //        {
        //            alice = new Model.IdentityUser
        //            {
        //                UserName = "AliceSmith@email.com",
        //                Email = "AliceSmith@email.com"
        //            };
        //            var result = userMgr.CreateAsync(alice, "Pass123$").Result;
        //            if (!result.Succeeded)
        //            {
        //                throw new Exception(result.Errors.First().Description);
        //            }

        //            result = userMgr.AddClaimsAsync(alice, new Claim[]{
        //                new Claim(JwtClaimTypes.Id, "Alice Smith"),
        //                new Claim(JwtClaimTypes.Role, "" ),
        //                new Claim(JwtClaimTypes.Scope, "Alice Smith"),
        //                new Claim(JwtClaimTypes.Name, "Alice Smith"),
        //                new Claim(JwtClaimTypes.GivenName, "Alice"),
        //                new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //                new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
        //                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
        //                new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
        //                new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
        //            }).Result;
        //            if (!result.Succeeded)
        //            {
        //                throw new Exception(result.Errors.First().Description);
        //            }
        //            Console.WriteLine("alice created");
        //        }
        //        else
        //        {
        //            Console.WriteLine("alice already exists");
        //        }

        //        var bob = userMgr.FindByNameAsync("BobSmith@email.com").Result;
        //        if (bob == null)
        //        {
        //            bob = new Model.IdentityUser
        //            {
        //                UserName = "BobSmith@email.com",
        //                Email = "BobSmith@email.com"
        //            };
        //            var result = userMgr.CreateAsync(bob, "Pass123$").Result;
        //            if (!result.Succeeded)
        //            {
        //                throw new Exception(result.Errors.First().Description);
        //            }

        //            result = userMgr.AddClaimsAsync(bob, new Claim[]{
        //                new Claim(JwtClaimTypes.Name, "Bob Smith"),
        //                new Claim(JwtClaimTypes.GivenName, "Bob"),
        //                new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //                new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
        //                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
        //                new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
        //                //new Claim(JwtClaimTypes.Role, [], ClaimValueTypes. ),
        //                new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
        //                new Claim("location", "somewhere")
        //            }).Result;
        //            if (!result.Succeeded)
        //            {
        //                throw new Exception(result.Errors.First().Description);
        //            }
        //            Console.WriteLine("bob created");
        //        }
        //        else
        //        {
        //            Console.WriteLine("bob already exists");
        //        }
        //    }
        //}


        public async static void EnsureSeedDataServer4(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                try {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    if (!context.Clients.AsQueryable().Any())
                    {
                        foreach (var client in Configuration.Clients.Get().ToList())
                        {
                           await context.AddClient(client.ToEntity());
                        }
                    }

                    var ressource = await context.IdentityResources.Find(_ => true).ToListAsync();
                    foreach (var resource in Configuration.Resources.GetIdentityResources().ToList())
                    {
                        if(!ressource.Any(r => r.Name == resource.Name)) {
                            await context.AddIdentityResource(resource.ToEntity());
                        }
                    }


                    if (!context.ApiResources.AsQueryable().Any())
                    {
                        foreach (var resource in Configuration.Resources.GetApiResources().ToList())
                        {
                            await context.AddApiResource(resource.ToEntity());
                        }
                    }
                } catch (Exception ex) {
                    var toto = ex;
                } 

            }
        }
    }
}
