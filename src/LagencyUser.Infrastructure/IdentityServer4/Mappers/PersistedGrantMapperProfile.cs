// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;
using LagencyUserApplication.Model;
using Models = IdentityServer4.Models;

namespace LagencyUserInfrastructure.IdentityServer4.Mappers
{
    /// <summary>
    /// AutoMapper Config for PersistedGrant
    /// Between Model and Entity
    /// <seealso cref="https://github.com/AutoMapper/AutoMapper/wiki/Configuration">
    /// </seealso>
    /// </summary>
    public class PersistedGrantMapperProfile : Profile
    {
        /// <summary>
        /// <see cref="PersistedGrantMapperProfile">
        /// </see>
        /// </summary>
        public PersistedGrantMapperProfile()
        {
            // entity to model
            CreateMap<PersistedGrant, Models.PersistedGrant>(MemberList.Destination);

            // model to entity
            CreateMap<Models.PersistedGrant, PersistedGrant>(MemberList.Source);
        }
    }
}
