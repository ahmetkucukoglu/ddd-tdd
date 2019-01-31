namespace DDDSample.Application.Meetup.Commands.CreateMeetup
{
    using DDDSample.Application.Infrastructure;
    using DDDSample.Domain.MeetupAggregate;
    using DDDSample.Domain.MeetupAggregate.Policies;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateMeetupCommandHandler : IRequestHandler<CreateMeetupCommand, string>
    {
        private readonly IMeetupRepository _meetupRepository;
        private readonly IMeetupPolicy _meetupPolicy;
        private readonly IIdentityService _identityService;

        public CreateMeetupCommandHandler(
            IMeetupRepository meetupRepository,
            IMeetupPolicy meetupPolicy,
            IIdentityService identityService)
        {
            _meetupRepository = meetupRepository;
            _meetupPolicy = meetupPolicy;
            _identityService = identityService;
        }

        public async Task<string> Handle(CreateMeetupCommand request, CancellationToken cancellationToken)
        {
            var location = new Location(request.Address);
            var meetup = new Meetup(_identityService.GetUserId(), request.Subject, request.When, request.Description, location, _meetupPolicy);

            await _meetupRepository.AddAsync(meetup);
            await _meetupRepository.UnitOfWork.SaveEntitiesAsync();

            return meetup.Id;
        }
    }
}
