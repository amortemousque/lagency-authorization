// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using MongoDB.Bson;
using System;
using System.Collections.Generic;

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
    }
}
