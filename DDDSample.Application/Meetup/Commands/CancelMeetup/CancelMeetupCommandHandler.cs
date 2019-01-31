namespace DDDSample.Application.Meetup.Commands.CancelMeetup
{
    using DDDSample.Application.Infrastructure;
    using DDDSample.Domain.MeetupAggregate;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CancelMeetupCommandHandler : IRequestHandler<CancelMeetupCommand, bool>
    {
        private readonly IMeetupRepository _meetupRepository;
        private readonly IIdentityService _identityService;

        public CancelMeetupCommandHandler(
            IMeetupRepository meetupRepository,
            IIdentityService identityService)
        {
            _meetupRepository = meetupRepository;
            _identityService = identityService;
        }

        public async Task<bool> Handle(CancelMeetupCommand request, CancellationToken cancellationToken)
        {
            var meetup = await _meetupRepository.GetAsync(request.MeetupId);

            if (meetup == null || meetup.OrganizerId != _identityService.GetUserId())
            {
                throw new NotFoundException("Meetup not found");
            }

            meetup.Cancel();

            await _meetupRepository.UnitOfWork.SaveEntitiesAsync();

            return true;
        }
    }
}
