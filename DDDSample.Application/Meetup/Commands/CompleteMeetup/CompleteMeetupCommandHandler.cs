namespace DDDSample.Application.Meetup.Commands.CompleteMeetup
{
    using DDDSample.Application.Infrastructure;
    using DDDSample.Domain.MeetupAggregate;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CompleteMeetupCommandHandler : IRequestHandler<CompleteMeetupCommand, bool>
    {
        private readonly IMeetupRepository _meetupRepository;
        private readonly IIdentityService _identityService;

        public CompleteMeetupCommandHandler(
            IMeetupRepository meetupRepository,
            IIdentityService identityService)
        {
            _meetupRepository = meetupRepository;
            _identityService = identityService;
        }

        public async Task<bool> Handle(CompleteMeetupCommand request, CancellationToken cancellationToken)
        {
            var meetup = await _meetupRepository.GetAsync(request.MeetupId);
        
            if (meetup == null || meetup.OrganizerId != _identityService.GetUserId())
            {
                throw new NotFoundException("Meetup not found");
            }

            meetup.Complete();

            await _meetupRepository.UnitOfWork.SaveEntitiesAsync();

            return true;
        }
    }
}
