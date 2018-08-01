﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using LagencyUser.Application.Contracts;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace LagencyUser.Application.Model
{
    public class ApiResource
    {
        public Guid Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<ApiSecret> Secrets { get; set; }
        public List<ApiScope> Scopes { get; set; }

        /// List of accociated user claims that should be included when this resource is requested.
        public List<string> UserClaims { get; set; }


        public void UpdateInfos(
            string displayName,
            string description,
            List<string> userClaims
        )
        {
            DisplayName = displayName;
            Description = description;
            UserClaims = userClaims;
        }


        public void Disable()
        {
            Enabled = false;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public ApiScope AddScope(string name, string description) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name must be specified", nameof(name));
            
            if (this.Scopes.Any(s => s.Name.ToLower() == name.ToLower()))
                throw new ArgumentException("An other api scope has the same name.", nameof(name));

            var scope = new ApiScope
            {
                Id = Guid.NewGuid(),
                ApiResourceId = this.Id,
                Name = name,
                DisplayName = name,
                Description = description,
            };
            this.Scopes.Add(scope);
            return scope;
        }

        public void DeleteScope(Guid scopeId)
        {
            var scope = this.Scopes.FirstOrDefault(s => s.Id == scopeId);
            if (Guid.Empty == scopeId)
                throw new ArgumentException("The name must be specified", nameof(scopeId));

            if (scope == null)
                throw new KeyNotFoundException();
            
            this.Scopes.Remove(scope);
        }

        public static class Factory
        {
            public static async Task<ApiResource> CreateNewEntry(
                      IApiResourceRepository repository,
                        string name,
                        string displayName
                     )
            {

                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("The name must be specified", nameof(name));

            
                if (!await repository.HasUniqName(name))
                    throw new ArgumentException("An other api resource has the same name.", nameof(name));
                

                var api = new ApiResource
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    DisplayName = displayName,
                    Enabled = true
                };


                return api;
            }
        }
    }
}
