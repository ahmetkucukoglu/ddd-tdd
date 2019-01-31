namespace DDDSample.Application.Meetup.Queries.GetOpenMeetups
{
    using DDDSample.Application.Infrastructure;
    using DDDSample.Domain.MeetupAggregate;
    using MediatR;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetMeetupsQueryHandler : IRequestHandler<GetMeetupsQuery, IEnumerable<GetMeetupQueryItem>>
    {
        private readonly IMeetupRepository _meetupRepository;
        private readonly IIdentityService _identityService;

        public GetMeetupsQueryHandler(
            IMeetupRepository meetupRepository,
            IIdentityService identityService)
        {
            _meetupRepository = meetupRepository;
            _identityService = identityService;
        }

        public Task<IEnumerable<GetMeetupQueryItem>> Handle(GetMeetupsQuery request, CancellationToken cancellationToken)
        {
            //TODO Validation

            var meetups = _meetupRepository.GetMeetups(_identityService.GetUserId());

            var result = meetups.Select((x) => MapMeetup(x)).AsEnumerable();

            return Task.FromResult(result);
        }

        #region Helpers

        private GetMeetupQueryItem MapMeetup(Meetup meetup)
        {
            var meetupDetail = new GetMeetupQueryItem
            {
                Address = meetup.Location.Address,
                Description = meetup.Description,
                Subject = meetup.Subject,
                When = meetup.When
            };

            return meetupDetail;
        }

        #endregion
    }
}
