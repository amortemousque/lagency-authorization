﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;
using LagencyUser.Application.Model;
using Models = IdentityServer4.Models;

namespace LagencyUserInfrastructure.IdentityServer4.Mappers
{
    public static class IdentityResourceMappers
    {
        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static Models.IdentityResource ToModel(this IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<Models.IdentityResource>(resource);
        }

        public static IdentityResource ToEntity(this Models.IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResource>(resource);
        }
    }
}