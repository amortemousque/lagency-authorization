﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;
using LagencyUser.Application.Model;
using System.Linq;
using Models = IdentityServer4.Models;


namespace LagencyUserInfrastructure.IdentityServer4.Mappers
{
    /// <summary>
    /// AutoMapper configuration for identity resource
    /// Between model and entity
    /// </summary>
    public class IdentityResourceMapperProfile : Profile
    {
        /// <summary>
        /// <see cref="IdentityResourceMapperProfile"/>
        /// </summary>
        public IdentityResourceMapperProfile()
        {
            // entity to model
            CreateMap<IdentityResource, Models.IdentityResource>(MemberList.Destination)
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(src => src.UserClaims.Select(x => x)));

            // model to entity
            CreateMap<Models.IdentityResource, IdentityResource>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x =>  x )));
        }
    }
}
