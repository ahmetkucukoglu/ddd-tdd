namespace DDDSample.Application.Meetup.Commands.JoinMeetup
{
    using DDDSample.Application.Infrastructure;
    using DDDSample.Domain.MeetupAggregate;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class JoinMeetupCommandHandler : IRequestHandler<JoinMeetupCommand, bool>
    {
        private readonly IMeetupRepository _meetupRepository;
        private readonly IIdentityService _identityService;

        public JoinMeetupCommandHandler(
            IMeetupRepository meetupRepository,
            IIdentityService identityService)
        {
            _meetupRepository = meetupRepository;
            _identityService = identityService;
        }

        public async Task<bool> Handle(JoinMeetupCommand request, CancellationToken cancellationToken)
        {
            var meetup = await _meetupRepository.GetAsync(request.MeetupId);
        
            if (meetup == null)
            {
                throw new NotFoundException("Meetup not found");
            }

            meetup.Join(_identityService.GetUserId());

            await _meetupRepository.UnitOfWork.SaveEntitiesAsync();

            return true;
        }
    }
}
