namespace DDDSample.Domain.MeetupAggregate.Policies
{
    using System;

    public interface IMeetupPolicy
    {
        void CheckCanDefineMeetup(string organizerId, DateTime when);
    }
}
