using Domain.Entities;
using Fitness.Application.Shedules.Commands;
using Fitness.Domain.Repositories;
using Fitness.Shared.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.TimeIntervals.Commands.Handlers
{
    internal sealed class UpdateTimeIntervalHandler : IRequestHandler<UpdateTimeInterval>
    {
        private readonly ITimeIntervalRepository _timeIntervalRepository;
        private readonly ILogger<UpdateTimeIntervalHandler> _logger;
        private readonly IHttpContextService _contextService;


        public UpdateTimeIntervalHandler(ITimeIntervalRepository timeIntervalRepository, ILogger<UpdateTimeIntervalHandler> logger, 
            IHttpContextService contextService)
        {
            _timeIntervalRepository = timeIntervalRepository;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task Handle(UpdateTimeInterval command, CancellationToken cancellationToken)
        {
            if(command.Start>=command.End)
            {
                throw new ArgumentException();
            }

            var userId = _contextService.GetCurrentUserId();

            await _timeIntervalRepository.UpdateAsync(new TimeInterval(command.Id, command.Start, command.End, userId));
        }
    }
}
