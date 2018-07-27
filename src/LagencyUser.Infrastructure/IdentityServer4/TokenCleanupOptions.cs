﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace LagencyUser.Infrastructure.IdentityServer4
{
    public class TokenCleanupOptions
    {
        public int Interval { get; set; } = 60;
    }
}