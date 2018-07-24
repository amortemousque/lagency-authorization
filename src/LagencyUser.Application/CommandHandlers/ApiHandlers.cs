using System.Threading.Tasks;
using MediatR;
using System;
using System.Threading;
using System.Collections.Generic;
using LagencyTalent.Domain.TalentAggregate;
using LagencyUser.Application.Commands;
using LagencyUserApplication.Model;
using LagencyUserInserstructure.Context;
using LagencyUser.Application.Contracts;

namespace LagencyUser.Application.CommandHandlers
{
    public class ApiHandlers : 
    IRequestHandler<CreateApiCommand, ApiResource>,
    IRequestHandler<UpdateApiCommand>
    {
        private readonly IApiResourceRepository _repository;
        public ApiHandlers(IApiResourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResource> Handle(CreateApiCommand message, CancellationToken cancellationToken)
        {
            var api = new ApiResource
            {
                Id = Guid.NewGuid(),
                Name = message.Name,
                Description = message.Description,
                DisplayName = message.DisplayName,
                Enabled = message.Enabled
            };

            await _repository.Add(api);
            return api;
        }



        public async Task Handle(UpdateApiCommand message, CancellationToken cancellationToken)
        {
            var api = _repository.GetById(message.Id);

            api.Name = message.Name;
            api.Enabled = message.Enabled;
            api.DisplayName = message.DisplayName;
            api.Description = message.Description;

            await _repository.SaveAsync(api);
        }

    }
}