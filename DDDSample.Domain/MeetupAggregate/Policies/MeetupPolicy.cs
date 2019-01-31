namespace DDDSample.Domain.MeetupAggregate.Policies
{
    using System;

    public class MeetupPolicy : IMeetupPolicy
    {
        private readonly IMeetupRepository _meetupRepository;

        public MeetupPolicy(IMeetupRepository meetupRepository)
        {
            _meetupRepository = meetupRepository;
        }

        public void CheckCanDefineMeetup(string organizerId, DateTime when)
        {
            var count = _meetupRepository.GetMeetupCountOfDay(organizerId, when);

            if (count > 0)
            {
                throw new MeetupDomainException("A maximum of one meetup is defined");
            }
        }
    }
}
